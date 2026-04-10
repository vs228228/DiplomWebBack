using DiplomWebBack.Api.Extensions;
using DiplomWebBack.Application.DTOs.User.Request;
using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Application.Usecases.Command.User;
using DiplomWebBack.Application.Usecases.Query.User;
using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Entities.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomWebBack.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Возвращает профиль пользователя. ПРОФИЛЬ ПОЛЬЗОВАТЕЛЯ МОГУТ ПОЛУЧИТЬ ВСЕ АВТОРИЗОВАННЫЕ ТИПОЧКИ
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{userId}/profile")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfileDto(Guid userId, CancellationToken cancellationToken)
        {
            var initiatorId = HttpContext.GetCurrentUserId();

            var command = new GetUserByIdCommand(userId, initiatorId);

            var user = await _mediator.Send(command, cancellationToken);

            return Ok(user);
        }

        /// <summary>
        /// Получить текущего юзера
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("me")]
        public async Task<ActionResult<UserProfileDto>> GetMeAsync(CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetCurrentUserId();

            var user = await _mediator.Send(new GetCurrentUserQuery(userId), cancellationToken);

            return Ok(user);
        }

        /// <summary>
        /// Возвращает всех не удалённых и активированных юзеров
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PaginatedList<UserProfileDto>>> GetAllUsersWithoutPagination([FromQuery] UserPaginatedRequestDto dto, CancellationToken cancellationToken)
        {
            var users = await _mediator.Send(new GetAllUsersWithoutPaginationQuery(dto), cancellationToken);

            return Ok(users);
        }

        /// <summary>
        /// Отправка резюме для получения скиллов
        /// </summary>
        /// <param name="resumeFile"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [HttpPost("extract-skills")]
        public async Task<ActionResult<SkillExtractionResponse>> ExtractSkills(IFormFile resumeFile, CancellationToken ct)
        {
            var userId = HttpContext.GetCurrentUserId();

            var result = await _mediator.Send(new ExtractSkillsCommand(resumeFile, userId), ct);

            return Ok(result);
        }

        /// <summary>
        /// Получение скиллов текущего юзера
        /// </summary>
        /// <param name="resumeFile"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("me/skills")]
        public async Task<ActionResult<SkillExtractionResponse>> GetMySkillsAsync(CancellationToken ct)
        {
            var userId = HttpContext.GetCurrentUserId();

            var result = await _mediator.Send(new GetUserSkillsQuery(userId, userId), ct);

            return Ok(result);
        }

        /// <summary>
        /// Получение скиллов юзера
        /// </summary>
        /// <param name="resumeFile"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("{userId}/skills")]
        public async Task<ActionResult<SkillExtractionResponse>> GetUserSkillsAsync([FromRoute] Guid userId, CancellationToken ct)
        {
            var initiatorId = HttpContext.GetCurrentUserId();

            var result = await _mediator.Send(new GetUserSkillsQuery(userId, initiatorId), ct);

            return Ok(result);
        }

        /// <summary>
        /// Обновить юзера
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        public async Task<ActionResult> UpdateUserAsync([FromRoute] Guid userId,  [FromBody] UpdateUserRequestDto request, CancellationToken ct)
        {
            var initiatorId = HttpContext.GetCurrentUserId();

            await _mediator.Send(new UpdateUserCommand(initiatorId, userId, request));

            return Ok();
        }

        /// <summary>
        /// Обновить
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("avatar")]
        public async Task<IActionResult> UploadAvatar(
            [FromForm] UploadAvatarRequest request,
            CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetCurrentUserId();

            await _mediator.Send(new UploadAvatarCommand(userId, request.File), cancellationToken);

            return Ok();
        }
    }
}
