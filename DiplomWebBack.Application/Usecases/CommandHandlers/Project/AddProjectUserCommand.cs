using DiplomWebBack.Application.DTOs.Project.Request.DiplomWebBack.Application.DTOs.Project.Request;
using MediatR;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.Project
{
    public record AddProjectUserCommand(AddProjectUserRequestDto Dto, Guid ProjectId, Guid UserId) : IRequest<Unit>;
}