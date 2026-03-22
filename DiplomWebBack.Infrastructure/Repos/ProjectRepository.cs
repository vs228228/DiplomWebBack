using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Entities.m2m;
using DiplomWebBack.Domain.Enums;
using DiplomWebBack.Domain.GetByIdModels;
using DiplomWebBack.Domain.Repos;
using DiplomWebBack.Infrastructure.Context;
using DiplomWebBack.Infrastructure.Extensions.ReposExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
            var query = _dbContext.Project
                .AsNoTracking()
                .Include(p => p.UserToProjects)
                    .ThenInclude(up => up.User)
                .Include(p => p.ProjectTags)
                    .ThenInclude(p => p.Tag)
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

        public async Task<ICollection<UserToProject>> GetUsersAsync(Guid projectId, CancellationToken cancellationToken)
        {
            return await _dbContext.UserToProject
                .AsNoTracking()
                .Where(up => up.ProjectId == projectId)
                .Include(up => up.User)
                .ToListAsync(cancellationToken);
        }

        public async Task<ICollection<UserToProject>> GetEmployeesAsync(Guid projectId, CancellationToken cancellationToken)
        {
            return await _dbContext.UserToProject
                .AsNoTracking()
                .Where(up => up.ProjectId == projectId && up.Role == ProjectRole.Employee)
                .Include(up => up.User)
                .ToListAsync(cancellationToken);
        }

        public async Task<ICollection<UserToProject>> GetManagersAsync(Guid projectId, CancellationToken cancellationToken)
        {
            return await _dbContext.UserToProject
                .AsNoTracking()
                .Where(up => up.ProjectId == projectId && up.Role == ProjectRole.Manager)
                .Include(up => up.User)
                .ToListAsync(cancellationToken);
        }

        public async Task<Project> GetProjectByIdAsync(GetProjectByIdModel model, CancellationToken cancellationToken)
        {
            return await _dbContext.Project
            .TrackChanges(model.TrackChanges)
            .Where(p => p.Id == model.Id)
            .IncludeTags(model.IncludeTags)
            .IncludeEmployees(model.IncludeEmployee)
            .IncludeCreator(model.IncludeCreatedBy)
            .FirstOrDefaultAsync(cancellationToken); 
        }

        public async Task<ICollection<Tag>> GetTagsAsync(Guid projectId, CancellationToken cancellationToken)
        {
            return await _dbContext.TagsToProjects
                .AsNoTracking()
                .Where(tp => tp.ProjectId == projectId)
                .Select(tp => tp.Tag)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsExistByConditionAsync(
            Expression<Func<Project, bool>> predicate,
            CancellationToken cancellationToken = default)
        {

            return await _dbContext.Project
                .AsNoTracking()
                .AnyAsync(predicate, cancellationToken);
        }
    }
}
