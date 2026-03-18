using DiplomWebBack.Domain.Entities;

namespace DiplomWebBack.Domain.Repos
{
    public interface IUserActivatorRepository : IBaseRepository<UserActivator>
    {
        Task<UserActivator> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool trackChanges = false);
        Task<UserActivator> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken, bool trackChanges = false);
        Task<PaginatedList<UserActivator>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
}
