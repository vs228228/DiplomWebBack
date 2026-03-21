using DiplomWebBack.Domain.Entities;

namespace DiplomWebBack.Domain.Repos
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, bool trackChanges = false, bool includeDeleted = false, bool includeInactive = false);
        Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default, bool trackChanges = false, bool includeDeleted = false, bool includeInactive = false);
        //  Task<PaginatedList<Tag>> GetAllAsync(int pageSize, int pageNumber, CancellationToken cancellationToken);
        Task<PaginatedList<User>> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
    }
}
