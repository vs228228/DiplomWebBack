using DiplomWebBack.Application.DTOs.Project.Request;
using MediatR;


namespace DiplomWebBack.Application.Usecases.Command.Projects
{
    public record UpdateProjectInfoCommand(UpdateProjectInfoRequestDto Dto, Guid ProjectId, Guid UserId) : IRequest<Unit>;
}
