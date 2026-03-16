using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DiplomWebBack.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        public string HashPassword(string password)
        {
            password = _passwordHasher.HashPassword(null, password);

            return password;
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var info = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            if (info != PasswordVerificationResult.Failed)
            {
                return true;
            }

            return false;
        }
    }
}
