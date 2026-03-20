using DiplomWebBack.Domain.Entities;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Query.Projects
{
    public record GetAllProjectsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<Project>>;
}
