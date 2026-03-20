using MediatR;

namespace DiplomWebBack.Application.Usecases.Command.Tags
{
    public record DeleteTagCommand(Guid Id) : IRequest;
}
