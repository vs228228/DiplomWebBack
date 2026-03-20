using DiplomWebBack.Application.DTOs.User.Response;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Query.Projects
{
    public record GetProjectUsersQuery(Guid ProjectId, Guid userId) : IRequest<IEnumerable<UserProjectResponseDto>>;
}
