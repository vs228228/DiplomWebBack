using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Repos;
using DiplomWebBack.Infrastructure.Context;
using DiplomWebBack.Infrastructure.Extensions.ReposExtensions;
using Microsoft.EntityFrameworkCore;

namespace DiplomWebBack.Infrastructure.Repos
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default, bool trackChanges = false, 
            bool includeDeleted = false, bool includeInactive = false)
        {
            return await _context.User.Where(u => u.Email == email)
                .OptionalWhere(when: !includeDeleted, u => u.IsDelete == false)
                .OptionalWhere(when: !includeInactive, u => u.IsActive == true)
                .TrackChanges(trackChanges)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, bool trackChanges = false,
            bool includeDeleted = false, bool includeInactive = false)
        {
            return await _context.User.Where(u => u.Id == id)
                .OptionalWhere(when: !includeDeleted, u => u.IsDelete == false)
                .OptionalWhere(when: !includeInactive, u => u.IsActive == true)
                .TrackChanges(trackChanges)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public new async Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = _context.User.Where(u => u.Id == userId).FirstOrDefault();

            if (user is null) return;

            user.IsDelete = true;

            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedList<User>> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _context.User
                .Where(u => !u.IsDelete && u.IsActive);

            var totalCount = await query.CountAsync(cancellationToken);

            List<User> users;

            if (pageNumber == -1)
            {
                users = await query.ToListAsync(cancellationToken);

                return new PaginatedList<User>
                {
                    List = users,
                    Meta = new MetaForPaginatedList
                    {
                        PageNumber = 1,
                        PageSize = totalCount,
                        TotalCount = totalCount,
                        TotalPageCount = 1
                    }
                };
            }

            users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var totalPageCount = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PaginatedList<User>
            {
                List = users,
                Meta = new MetaForPaginatedList
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPageCount = totalPageCount
                }
            };
        }

        public async Task<bool> AreAllUsersExistAsync(
            IEnumerable<Guid> userIds,
            CancellationToken cancellationToken = default)
        {
            var ids = userIds.Distinct().ToList();

            var count = await _context.User
                .Where(u => ids.Contains(u.Id) && !u.IsDelete && u.IsActive)
                .CountAsync(cancellationToken);

            return count == ids.Count;
        }
    }
}
