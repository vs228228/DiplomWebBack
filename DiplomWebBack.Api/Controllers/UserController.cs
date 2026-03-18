using DiplomWebBack.Api.Extensions;
using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Application.Usecases.Query.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomWebBack.Api.Controllers
{
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
        [Authorize]
        [HttpGet("{userId}/profile")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfileDto(Guid userId, CancellationToken cancellationToken)
        {
            var initiatorId = HttpContext.GetCurrentUserId();

            var command = new GetUserByIdCommand(userId, initiatorId);

            var user = await _mediator.Send(command, cancellationToken);

            return Ok(user);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserProfileDto>> GetMeAsync(CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetCurrentUserId();

            var user = await _mediator.Send(new GetCurrentUserQuery(userId), cancellationToken);

            return Ok(user);
        }
    }
}
