using DiplomWebBack.Application.DTOs.User.Response;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Query.User
{
    public record GetUserByIdCommand(Guid UserId, Guid InitiatorId) : IRequest<UserProfileDto>;
}
