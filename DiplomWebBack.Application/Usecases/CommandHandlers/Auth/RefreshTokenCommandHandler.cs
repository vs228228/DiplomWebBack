using DiplomWebBack.Application.Usecases.Command.Auth;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Interfaces;
using DiplomWebBack.Domain.Repos;
using MediatR;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.Auth
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public  async Task<string> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Dto.UserId, cancellationToken);

            if(user is null || user.RefreshToken != request.Dto.RefreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new UnauthorizedException("Не авторизован");
            }

            var accessToken = _tokenService.GenerateAccessToken(user);

            return accessToken;
        }
    }
}
