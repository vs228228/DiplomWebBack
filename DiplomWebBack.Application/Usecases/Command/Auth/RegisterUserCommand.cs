using DiplomWebBack.Application.DTOs.Auth.Request;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Command.Auth
{
    public sealed record RegisterUserCommand(RegisterRequestDto dto) : IRequest<Unit>;
}
