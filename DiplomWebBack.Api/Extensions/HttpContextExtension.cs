using DiplomWebBack.Domain.CustomExceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DiplomWebBack.Api.Extensions
{

    public static class HttpContextExtensions
    {
        public static Guid GetCurrentUserId(this HttpContext httpContext)
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedException("User ID claim not found");

            return Guid.Parse(userIdClaim.Value);
        }

        public static string GetRefreshToken(this HttpContext httpContext)
        {
            if (!httpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                throw new UnauthorizedException("Refresh token not found");

            return refreshToken;
        }

        /// <summary>
        /// Пытается получить userId из истекшего accessToken.
        /// Не проверяет срок действия.
        /// </summary>
        public static Guid? GetUserIdFromExpiredToken(this HttpContext httpContext)
        {
            var token = httpContext.Request.Cookies["accessToken"]
                        ?? httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrWhiteSpace(token))
                return null;



            var handler = new JwtSecurityTokenHandler();
            try
            {
                var jwtToken = handler.ReadJwtToken(token);

                foreach (var claim in jwtToken.Claims)
                {
                    Console.WriteLine($"{claim.Type}: {claim.Value}");
                }

                // Здесь ищем именно ClaimTypes.NameIdentifier
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c =>
                 c.Type == ClaimTypes.NameIdentifier ||
                 c.Type == "nameid" ||
                 c.Type == "sub");
                if (userIdClaim == null)
                    return null;

                return Guid.Parse(userIdClaim.Value);
            }
            catch
            {
                return null;
            }
        }

    }
}
