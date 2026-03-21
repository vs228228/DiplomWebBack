using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Repos;

namespace DiplomWebBack.DomainRepos.Repos
{
    public interface ITagsRepository : IBaseRepository<Tag>
    {
        Task<Tag> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, bool trackChanges = false);
        Task<Tag> GetByTitleAsync(string name, CancellationToken cancellationToken = default, bool trackChanges = false);
        Task<PaginatedList<Tag>> GetAllAsync(int pageSize, int pageNumber, string search, CancellationToken cancellationToken);
        Task<List<Tag>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    }
}
