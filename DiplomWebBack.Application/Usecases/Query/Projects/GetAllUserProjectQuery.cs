using DiplomWebBack.Application.DTOs.Project.Response;
using DiplomWebBack.Domain.Entities;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Query.Projects
{
    public sealed record GetAllUserProjectQuery(Guid UserId, bool IncludeCreatetProjects) : IRequest<ICollection<ProjectResponseDto>>;
}
