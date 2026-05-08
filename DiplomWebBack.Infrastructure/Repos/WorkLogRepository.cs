using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Repos;
using DiplomWebBack.Infrastructure.Context;
using DiplomWebBack.Infrastructure.Extensions.ReposExtensions;
using Microsoft.EntityFrameworkCore;

namespace DiplomWebBack.Infrastructure.Repos
{
    public class WorkLogRepository : BaseRepository<WorkLog>, IWorkLogRepository
    {
        private readonly AppDbContext _dbContext;

        public WorkLogRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WorkLog> GetWorkLogById(Guid Id, CancellationToken cancellationToken)
        {
            return await _dbContext.WorkLogs.Where(wl => wl.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<List<WorkLog>> GetWorkLogs(
            DateOnly? startDate = null,
            DateOnly? endDate = null,
            Guid? userId = null,
            Guid? projectId = null)
        {
            return await _dbContext.WorkLogs
                .OptionalWhere(startDate is not null, x => x.WorkDate >= startDate)
                .OptionalWhere(endDate is not null, x => x.WorkDate <= endDate)
                .OptionalWhere(userId is not null, x => x.UserId == userId)
                .OptionalWhere(projectId is not null, x => x.ProjectId == projectId)
                .ToListAsync();
        }
    }
}
