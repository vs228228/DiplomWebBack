using DiplomWebBack.Application.DTOs.User.Request;

namespace DiplomWebBack.Application.DTOs.Project.Request
{
    public record UpdateProjectUsersRequestDto(IEnumerable<UserProjectShortRequest> Users);
}