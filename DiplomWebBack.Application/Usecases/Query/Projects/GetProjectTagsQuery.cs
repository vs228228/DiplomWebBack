using DiplomWebBack.Domain.Entities;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Query.Projects
{
    public record GetProjectTagsQuery(Guid projectId, Guid userId) : IRequest<IEnumerable<Tag>>;
}
