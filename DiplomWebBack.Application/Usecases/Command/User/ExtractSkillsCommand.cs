using DiplomWebBack.Application.DTOs.User.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DiplomWebBack.Application.Usecases.Command.User
{
    public record ExtractSkillsCommand(IFormFile ResumeFile, Guid UserId, Guid InitiatorId) : IRequest<SkillResponseDto>;
}
