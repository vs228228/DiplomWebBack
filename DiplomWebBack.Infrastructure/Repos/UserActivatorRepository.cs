using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Repos;
using DiplomWebBack.Infrastructure.Context;
using DiplomWebBack.Infrastructure.Extensions.ReposExtensions;
using Microsoft.EntityFrameworkCore;

namespace DiplomWebBack.Infrastructure.Repos
{
    public class UserActivatorRepository : BaseRepository<UserActivator>, IUserActivatorRepository
    {
        public UserActivatorRepository(AppDbContext context) : base(context)
        {
          
        }

        public async Task<PaginatedList<UserActivator>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var list = await _context.Activators
                .AsNoTracking()
                .Where(ua => ua.AprovedById == null)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(ua => ua.User)
                .ToListAsync(cancellationToken);

            int totalCount = await _context.Activators.Where(a => a.AprovedById == null).CountAsync();

            return new PaginatedList<UserActivator>()
            {
                List = list,
                Meta = new MetaForPaginatedList()
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPageCount = (int)Math.Ceiling(totalCount / (double)pageSize)
                },
            };
        }

        public async Task<UserActivator> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool trackChanges = false)
        {
            return await _context.Activators
                .TrackChanges(trackChanges)
                .Where(a => a.Id == id)
                .Include(ua => ua.User)
                .FirstOrDefaultAsync();
        }

        public async Task<UserActivator> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken, bool trackChanges = false)
        {
            return await _context.Activators
                .TrackChanges(trackChanges)
                .Where(a => a.UserId == userId)
                .Include(ua => ua.User)
                .FirstOrDefaultAsync();
        }
    }
}
