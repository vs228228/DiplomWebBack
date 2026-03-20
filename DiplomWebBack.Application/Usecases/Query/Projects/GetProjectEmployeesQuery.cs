using DiplomWebBack.Application.DTOs.User.Response;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Query.Projects
{
    public record GetProjectEmployeesQuery(Guid urojectId, Guid userId) : IRequest<IEnumerable<UserProjectResponseDto>>;
}
