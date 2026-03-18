using DiplomWebBack.Domain.CustomExceptions;
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
    }
}
