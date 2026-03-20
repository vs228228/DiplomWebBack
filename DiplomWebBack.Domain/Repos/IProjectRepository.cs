using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.GetByIdModels;

namespace DiplomWebBack.Domain.Repos
{
    public interface IProjectRepository : IBaseRepository<Project>
    {
        Task<PaginatedList<Project>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<ICollection<User>> GetUsersAsync(Guid projectId, CancellationToken cancellationToken);
        Task<ICollection<User>> GetEmployeesAsync(Guid projectId, CancellationToken cancellationToken);
        Task<ICollection<User>> GetManagersAsync(Guid projectId, CancellationToken cancellationToken);
        Task<Project> GetProjectByIdAsync(GetProjectByIdModel model, CancellationToken cancellationToken);
        Task<ICollection<Tag>> GetTagsAsync(Guid projectId, CancellationToken cancellationToken);
    }
}
