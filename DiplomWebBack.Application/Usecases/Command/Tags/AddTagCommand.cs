using MediatR;

namespace DiplomWebBack.Application.Usecases.Command.Tags
{
    public record AddTagCommand(string Title) : IRequest<Guid>;
}
