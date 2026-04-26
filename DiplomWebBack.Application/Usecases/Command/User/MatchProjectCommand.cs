using DiplomWebBack.Application.DTOs.User.Request;
using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Domain.Entities.Responses;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Command.User
{
    public sealed record MatchProjectCommand(Guid InitiatorId, MatchProjectsRequestDto Request) : IRequest<List<ProjectMatchResponseDto>>;
}
