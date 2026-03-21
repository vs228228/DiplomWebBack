using DiplomWebBack.Domain.Entities;

namespace DiplomWebBack.Application.Services.Interfaces
{
    public interface IUserVerificationService
    {
        Task<User> CheckIfUserValidAndGetAsync(Guid userId, CancellationToken cancellationToken);
    }
}
