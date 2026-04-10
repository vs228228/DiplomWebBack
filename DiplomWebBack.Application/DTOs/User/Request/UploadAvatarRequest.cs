using Microsoft.AspNetCore.Http;

namespace DiplomWebBack.Application.DTOs.User.Request
{
    public sealed record UploadAvatarRequest(IFormFile File);
}
