using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Repos;

namespace DiplomWebBack.Application.Services.Implementation
{
    public class UserVerificationService : IUserVerificationService
    {
        private readonly IUserRepository _userRepository;

        public UserVerificationService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        
        public async Task<User> CheckIfUserValidAndGetAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if(user is null)
            {
                throw new NotFoundException($"Юзер с айди {userId} не найден");
            }

            return user;
        }
    }
}
