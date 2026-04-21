using Microsoft.AspNetCore.Http;

namespace DiplomWebBack.Application.DTOs.Project.Request
{
    public record UpdateProjectInfoRequestDto(string Title, string Description, IFormFile? TechnicalTask = null);
}