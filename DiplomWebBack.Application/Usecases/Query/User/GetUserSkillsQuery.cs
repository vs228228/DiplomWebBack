using DiplomWebBack.Domain.Entities.Responses;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Query.User
{
    public sealed record GetUserSkillsQuery(Guid UserId, Guid InitiatorId, string SearchBy) : IRequest<SkillExtraction>;
}
