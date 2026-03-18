using DiplomWebBack.Application.DTOs.Tags.Responses;
using DiplomWebBack.Application.Usecases.Command.Auth;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Interfaces;
using DiplomWebBack.Domain.Repos;
using MediatR;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.Auth
{
    public class LoginIntoSystemCommandHandler : IRequestHandler<LoginIntoSystemCommand, TokensResponseDto>
    {
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;

        public LoginIntoSystemCommandHandler(ITokenService tokenService,
            IPasswordHasher passwordHasher,
            IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<TokensResponseDto> Handle(LoginIntoSystemCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.request.Login, cancellationToken);

            if(user is null)
            {
                throw new NotFoundException("Пользователь не найден");
            }

            var isEq = _passwordHasher.VerifyPassword(user.PasswordHash, request.request.Password);

            if (!isEq)
            {
                throw new BadRequestException("Неверный логин или пароль");
            }

            if (user.RefreshToken is null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {

                var refreshToken = _tokenService.GenerateRefreshToken(user.Id);

                user.RefreshToken = refreshToken.refreshToken;
                user.RefreshTokenExpiryTime = refreshToken.ExpiresAt;
            }

            var accessToken = _tokenService.GenerateAccessToken(user);

            await _userRepository.UpdateAsync(user);

            var tokens = new TokensResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken,
            };

            return tokens;
        }
    }
}
