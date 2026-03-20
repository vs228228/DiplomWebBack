using DiplomWebBack.Application.DTOs.User.Response;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Query.Projects
{
    public record GetProjectManagersQuery(Guid projectId, Guid userId) : IRequest<IEnumerable<UserProjectResponseDto>>;
}
