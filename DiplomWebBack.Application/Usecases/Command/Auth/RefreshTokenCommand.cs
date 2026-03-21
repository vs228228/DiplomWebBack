using DiplomWebBack.Application.DTOs.Auth.Request;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Command.Auth
{
    public class RefreshTokenCommand : IRequest<string>
    {
        public RefreshTokenRequestDto Dto { get; set; }
    }
}
