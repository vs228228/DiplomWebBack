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

        public async Task<PaginatedList<Tag>> GetAllAsync(
            int pageSize,
            int pageNumber,
            string search,
            CancellationToken cancellationToken)
        {
            var query = _context.Tags
                .TrackChanges(false)
                .OptionalWhere(
                    when: !string.IsNullOrWhiteSpace(search),
                    t => t.Title.ToLower().Contains(search.ToLower()));

            var totalCount = await query.CountAsync(cancellationToken);

            List<Tag> tags;

            if (pageNumber == -1)
            {
                tags = await query.ToListAsync(cancellationToken);
            }
            else
            {
                tags = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);
            }

            return new PaginatedList<Tag>
            {
                List = tags,
                Meta = new MetaForPaginatedList()
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPageCount = pageNumber == -1
                        ? 1
                        : (int)Math.Ceiling(totalCount / (double)pageSize)
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

        public async Task<List<Tag>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken, bool trackChanges = false)
        {
            return await _context.Tags
                .TrackChanges(trackChanges)
                .Where(p => ids.Contains(p.Id))
                .ToListAsync(cancellationToken);
        }

        /* public new async Task UpdateAsync(Tag tag, CancellationToken cancellationToken = default)
         {

             _context.Tags.Update(tag);

             await _context.SaveChangesAsync(cancellationToken);
         }*/

        public async Task RemoveProjectTagsAsync(Guid projectId)
        {
            var existing = _context.TagsToProjects.Where(x => x.ProjectId == projectId);
            _context.TagsToProjects.RemoveRange(existing);

            await _context.SaveChangesAsync();
        }
    }
}