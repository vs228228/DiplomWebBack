using DiplomWebBack.Application.DTOs.Tags.Requests;
using DiplomWebBack.Application.DTOs.Tags.Responses;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Command
{
    public sealed record LoginIntoSystemCommand(LoginRequestDto request) : IRequest<TokensResponseDto>;
}
