using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Enums;
using DiplomWebBack.Domain.GetByIdModels;
using DiplomWebBack.Domain.Repos;
using DiplomWebBack.Infrastructure.Context;
using DiplomWebBack.Infrastructure.Extensions.ReposExtensions;
using Microsoft.EntityFrameworkCore;

namespace DiplomWebBack.Infrastructure.Repos
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        private readonly AppDbContext _dbContext;

        public ProjectRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<Project>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _dbContext.Projects
                .AsNoTracking()
                .Include(p => p.UserToProjects)
                    .ThenInclude(up => up.User)
                .Include(p => p.Tags)
                .Include(p => p.CreatedBy)
                .OrderBy(p => p.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<Project>
            {
                List = list,
                Meta = new MetaForPaginatedList
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPageCount = (int)Math.Ceiling(totalCount / (double)pageSize)
                }
            };
        }

        public async Task<ICollection<User>> GetUsersAsync(Guid projectId, CancellationToken cancellationToken)
        {
            return await _dbContext.UserToProjects
                .AsNoTracking()
                .Where(up => up.ProjectId == projectId)
                .Select(up => up.User)
                .ToListAsync(cancellationToken);
        }

        public async Task<ICollection<User>> GetEmployeesAsync(Guid projectId, CancellationToken cancellationToken)
        {
            return await _dbContext.UserToProjects
                .AsNoTracking()
                .Where(up => up.ProjectId == projectId && up.Role == ProjectRole.Employee)
                .Select(up => up.User)
                .ToListAsync(cancellationToken);
        }

        public async Task<ICollection<User>> GetManagersAsync(Guid projectId, CancellationToken cancellationToken)
        {
            return await _dbContext.UserToProjects
                .AsNoTracking()
                .Where(up => up.ProjectId == projectId && up.Role == ProjectRole.Manager)
                .Select(up => up.User)
                .ToListAsync(cancellationToken);
        }

        public async Task<Project> GetProjectByIdAsync(GetProjectByIdModel model, CancellationToken cancellationToken)
        {
            return await _dbContext.Projects
            .TrackChanges(model.TrackChanges)
            .Where(p => p.Id == model.Id)
            .Include(p => p.UserToProjects)
                .ThenInclude(up => up.User)
            .Include(p => p.Tags)
            .Include(p => p.CreatedBy)
            .FirstOrDefaultAsync(cancellationToken); 
        }

        public async Task<ICollection<Tag>> GetTagsAsync(Guid projectId, CancellationToken cancellationToken)
        {
            return await _dbContext.Projects
                .AsNoTracking()
                .Where(p => p.Id == projectId)
                .SelectMany(p => p.Tags)
                .ToListAsync(cancellationToken);
        }
    }
}
