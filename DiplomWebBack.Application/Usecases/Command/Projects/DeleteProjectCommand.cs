using MediatR;

namespace DiplomWebBack.Application.Usecases.Command.Projects
{
    public record DeleteProjectCommand(Guid ProjectId, Guid UserId) : IRequest;
}
