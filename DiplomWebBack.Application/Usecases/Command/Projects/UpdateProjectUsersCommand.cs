using DiplomWebBack.Application.DTOs.Project.Request;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Command.Projects
{
    public record UpdateProjectUsersCommand(UpdateProjectUsersRequestDto Dto, Guid ProjectId, Guid UserId) : IRequest<Unit>;
}
