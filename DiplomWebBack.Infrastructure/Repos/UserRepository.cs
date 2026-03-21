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

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.User.Where(u => u.IsDelete == false && u.IsActive == true).ToListAsync(cancellationToken);
        }
    }
}
