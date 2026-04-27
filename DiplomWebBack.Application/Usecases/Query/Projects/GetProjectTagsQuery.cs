using DiplomWebBack.Application.DTOs.Project.Response;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Query.Projects
{
    public record GetProjectTagsQuery(Guid ProjectId, Guid UserId) : IRequest<AllProjectTagsResponse>;
}
