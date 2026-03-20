using MediatR;

namespace DiplomWebBack.Application.Usecases.Command.UserActivator
{
    public record class ActivateUserCommand(Guid UserId, Guid InitiatorId) : IRequest<Unit>;
}
