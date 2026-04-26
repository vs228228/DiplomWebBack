namespace DiplomWebBack.Application.DTOs.Project.Request
{
    public record UpdateProjectTagsRequestDto(IEnumerable<ProjectTagRequestDto> Tags);
}