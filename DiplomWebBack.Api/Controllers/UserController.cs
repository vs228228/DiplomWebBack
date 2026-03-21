using DiplomWebBack.Api.Extensions;
using DiplomWebBack.Application.DTOs.User.Request;
using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Application.Usecases.Query.User;
using DiplomWebBack.Domain.Entities;
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
        public async Task<ActionResult<PaginatedList<UserProfileDto>>> GetAllUsersWithoutPagination([FromQuery] UserPaginatedRequestDto dto,CancellationToken cancellationToken)
        {
            var users = await _mediator.Send(new GetAllUsersWithoutPaginationQuery(dto), cancellationToken);

            return Ok(users);
        }
    }
}
