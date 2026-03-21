using DiplomWebBack.Domain.Entities;
using DiplomWebBack.DomainRepos.Repos;
using DiplomWebBack.Infrastructure.Context;
using DiplomWebBack.Infrastructure.Extensions.ReposExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DiplomWebBack.Infrastructure.Repos
{
    public class TagsRepository : BaseRepository<Tag>, ITagsRepository
    {
        public TagsRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Tag>> GetAllAsync(int pageSize, int pageNumber, string search, CancellationToken cancellationToken)
        {
            var tags = await _context.Tags
                .TrackChanges(false)
                .OptionalWhere(when: !string.IsNullOrWhiteSpace(search), t => t.Title.ToLower().Contains(search.ToLower()))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var totalCount = await _context.Tags.CountAsync(cancellationToken);

            return new PaginatedList<Tag>
            {
                List = tags,
                Meta = new MetaForPaginatedList()
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPageCount = (int)Math.Ceiling(totalCount / (double)pageSize)
                },
            };
        }

        public async Task<Tag> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, bool trackChanges = false)
        {
            return await _context.Tags
                .TrackChanges(trackChanges)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<Tag> GetByTitleAsync(string name, CancellationToken cancellationToken = default, bool trackChanges = false)
        {
            return await _context.Tags
                .TrackChanges(trackChanges)
                .FirstOrDefaultAsync(t => t.Title == name, cancellationToken);
        }

    public async Task<List<Tag>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        {
            return await _context.Tags
                .Where(p => ids.Contains(p.Id))
                .ToListAsync(cancellationToken);
        }
    }
}