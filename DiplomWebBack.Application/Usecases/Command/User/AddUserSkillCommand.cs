using DiplomWebBack.Application.DTOs.User.Request;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Command.User
{
    public record AddUserSkillCommand(Guid UserId, AddUserSkillRequestDto Request) : IRequest<Guid>;
}
