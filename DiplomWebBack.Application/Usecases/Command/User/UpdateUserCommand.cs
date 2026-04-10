using DiplomWebBack.Application.DTOs.User.Request;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Command.User
{
    public record UpdateUserCommand(Guid InitiatorId, Guid UserId, UpdateUserRequestDto Request) : IRequest<Unit>;
}
