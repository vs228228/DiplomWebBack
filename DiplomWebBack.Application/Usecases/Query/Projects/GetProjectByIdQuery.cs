using DiplomWebBack.Domain.Entities;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Query.Projects
{
    public record GetProjectByIdQuery(Guid Id, Guid userId) : IRequest<Project?>;
}
