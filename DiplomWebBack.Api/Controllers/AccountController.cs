using DiplomWebBack.Application.DTOs.Tags.Requests;
using DiplomWebBack.Application.Usecases.Command;
using MediatR;
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
        public async Task<IActionResult> LoginIntoSystemAsync([FromBody] LoginRequestDto dto, CancellationToken cancellationToken = default)
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
        public async Task<IActionResult> RegisterNewUserAsync([FromBody] LoginRequestDto dto, CancellationToken cancellationToken = default)
        {
            var tokens = await _mediator.Send(new LoginIntoSystemCommand(dto), cancellationToken);

            return Ok(tokens);
        }
    }
}
