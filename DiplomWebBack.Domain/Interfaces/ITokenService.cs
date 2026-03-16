using DiplomWebBack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Domain.Interfaces
{
    public interface ITokenService
    {
        (string refreshToken, DateTimeOffset ExpiresAt) GenerateRefreshToken(Guid userId);
        string GenerateAccessToken(User user);
    }
}
