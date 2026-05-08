using DiplomWebBack.Domain.Entities;

namespace DiplomWebBack.Domain.Repos
{
    public interface IWorkLogRepository : IBaseRepository<WorkLog>
    {
        Task<List<WorkLog>> GetWorkLogs(
    DateOnly? startDate = null,
    DateOnly? endDate = null,
    Guid? userId = null,
    Guid? projectId = null);
        Task<WorkLog> GetWorkLogById (Guid Id, CancellationToken cancellationToken);
    }
}
