using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Interfaces;
using DiplomWebBack.Infrastructure.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DiplomWebBack.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _secret;
        private readonly int _accessTokenExpirationMinutes;
        private readonly int _refreshTokenExpirationDays;

        public TokenService(JwtSettings jwtSettings)
        {
            _secret = jwtSettings.Secret;
            _accessTokenExpirationMinutes = jwtSettings.AccessTokenExpirationMinutes;
            _refreshTokenExpirationDays = jwtSettings.RefreshTokenExpirationDays;
        }

        public (string refreshToken, DateTimeOffset ExpiresAt) GenerateRefreshToken(Guid userId)
        {
            var rng = new Random();
            var bytes = new byte[32];
            rng.NextBytes(bytes);
            var tokenInfo = Convert.ToBase64String(bytes);


            return (
                refreshToken: tokenInfo,
                ExpiresAt: DateTimeOffset.UtcNow.AddDays(_refreshTokenExpirationDays)
             );
        }

        public string GenerateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var role = user.Role.ToString();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
