using DiplomWebBack.Application.DTOs.Project.Request;
using DiplomWebBack.Application.DTOs.Project.Response;
using DiplomWebBack.Domain.Entities;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Query.Projects
{
    public record GetAllProjectsQuery(GetAllProjectsRequestDto Request) : IRequest<PaginatedList<ProjectResponseDto>>;
}
