using DiplomWebBack.Domain.CustomExceptions;
using System.Net;
using System.Text.Json;

namespace DiplomWebBack.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedException => (int)HttpStatusCode.Unauthorized,
                BadRequestException => (int)HttpStatusCode.BadRequest,
                ForbiddenException => (int)HttpStatusCode.Forbidden,
                HttpRequestException => (int)HttpStatusCode.BadGateway,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var errorResponse = CreateErrorResponse(context.Response.StatusCode, ex);

            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

        private object CreateErrorResponse(int statusCode, Exception ex)
        {
            return statusCode switch
            {
                (int)HttpStatusCode.NotFound => new { statusCode = statusCode, message = ex.Message },
                (int)HttpStatusCode.Unauthorized => new { statusCode = statusCode, message = ex.Message },
                (int)HttpStatusCode.BadRequest => new { statusCode = statusCode, message = ex.Message },
                (int)HttpStatusCode.Forbidden => new {statusCode = statusCode, message = ex.Message },
                (int)HttpStatusCode.BadGateway => new { statusCode = statusCode, message = ex.Message },
                _ => new { statusCode = statusCode, message = ex.Message}
            };
        }
    }
}
