using DiplomWebBack.Application.DTOs.Project.Request;
using DiplomWebBack.Domain.Entities;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Command.Projects
{
    public record CreateProjectCommand(ProjectCreateRequestDto Project, Guid UserId) : IRequest<Project>;
}
