namespace DiplomWebBack.Application.DTOs.Project.Request
{
    public record UpdateProjectTagsRequestDto(IEnumerable<Guid> Tags);
}