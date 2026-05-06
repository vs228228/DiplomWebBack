using MediatR;

namespace DiplomWebBack.Application.Usecases.Command.UserActivator
{
   public record CanlcelUserCommand(Guid UserId, Guid InitiatorId) : IRequest<Unit>;
}
