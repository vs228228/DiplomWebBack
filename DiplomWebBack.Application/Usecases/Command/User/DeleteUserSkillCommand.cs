using MediatR;

namespace DiplomWebBack.Application.Usecases.Command.User
{
    public record class DeleteUserSkillCommand(Guid UserId, Guid SkillId) : IRequest<Unit>;
}
