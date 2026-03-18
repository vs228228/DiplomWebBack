using DiplomWebBack.Application.DTOs.Auth.Request;
using DiplomWebBack.Application.DTOs.Tags.Requests;
using DiplomWebBack.Application.DTOs.Tags.Responses;
using DiplomWebBack.Application.Usecases.Command.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomWebBack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Вернёт пару токенов + время жизни рефреша. Не работает до конца
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<TokensResponseDto>> LoginIntoSystemAsync([FromBody] LoginRequestDto dto, CancellationToken cancellationToken = default)
        {
            var tokens = await _mediator.Send(new LoginIntoSystemCommand(dto), cancellationToken);

            return Ok(tokens);
        }

        /// <summary>
        /// Позволит создать юзера, однако войти нужно будет всё равно. Вернёт тольк ок. Ещё не реализован
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> RegisterNewUserAsync([FromBody] RegisterRequestDto dto, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new RegisterUserCommand(dto), cancellationToken);

            return Ok();
        }

        /// <summary>
        /// Проверить работает ли авторизация
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("is-me-auth")]
        public async Task<ActionResult> CheckIsUserAuth(CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}
