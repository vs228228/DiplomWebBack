using DiplomWebBack.Api.Extensions;
using DiplomWebBack.Application.DTOs.Auth.Request;
using DiplomWebBack.Application.DTOs.Tags.Responses;
using DiplomWebBack.Application.Usecases.Command.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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
        /// Вернёт пару токенов + время жизни рефреша
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<TokensResponseDto>> LoginIntoSystemAsync([FromBody] LoginRequestDto dto, CancellationToken cancellationToken = default)
        {
            var tokens = await _mediator.Send(new LoginIntoSystemCommand(dto), cancellationToken);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // HTTP → ОБЯЗАТЕЛЬНО false
                SameSite = SameSiteMode.Lax, // безопасный вариант для HTTP
                Expires = DateTimeOffset.UtcNow.AddDays(7) // подгони под refresh token
            };

            // Access token (обычно короткоживущий)
            Response.Cookies.Append("accessToken", tokens.AccessToken, cookieOptions);

            // Refresh token (долгоживущий)
            Response.Cookies.Append("refreshToken", tokens.RefreshToken, cookieOptions);

            return Ok(tokens);
        }

        /// <summary>
        /// Позволит создать юзера, однако войти нужно будет всё равно. Вернёт тольк ок.
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
         
        /// <summary>
        /// Обновить accessToken
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("refreshToken")]
        public async Task<ActionResult<string>> RefreshTokenAsync(CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetCurrentUserId();

            var refreshToken = HttpContext.GetRefreshToken();

            var dto = new RefreshTokenRequestDto() { RefreshToken = refreshToken, UserId = userId };

            var accessToken = await _mediator.Send(new RefreshTokenCommand() { Dto = dto }, cancellationToken);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // HTTP → ОБЯЗАТЕЛЬНО false
                SameSite = SameSiteMode.Lax, // безопасный вариант для HTTP
                Expires = DateTimeOffset.UtcNow.AddDays(7) // подгони под refresh token
            };

            // Access token (обычно короткоживущий)
            Response.Cookies.Append("accessToken", accessToken, cookieOptions);

            return Ok(accessToken);
        }
    }
}
