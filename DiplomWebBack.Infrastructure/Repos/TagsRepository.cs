using DiplomWebBack.Domain.Entities;
using DiplomWebBack.DomainRepos.Repos;
using DiplomWebBack.Infrastructure.Context;
using DiplomWebBack.Infrastructure.Extensions.ReposExtensions;
using Microsoft.EntityFrameworkCore;

namespace DiplomWebBack.Infrastructure.Repos
{
    public class TagsRepository : BaseRepository<Tag>, ITagsRepository
    {
        public TagsRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Tag>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Tags
                .TrackChanges(false)
                .ToListAsync(cancellationToken);
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
    }
}