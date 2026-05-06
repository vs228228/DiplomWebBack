using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Entities.m2m;
using DiplomWebBack.Domain.GetByIdModels;
using System.Linq.Expressions;

namespace DiplomWebBack.Domain.Repos
{
    public interface IProjectRepository : IBaseRepository<Project>
    {
        Task<PaginatedList<Project>> GetAllPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken,
            string search = "",
            IEnumerable<Guid> filtredByUser = null,
            IEnumerable<Guid> filtredByTags = null,
            Guid? ExceptUser = null);

        Task<ICollection<UserToProject>> GetUsersAsync(Guid projectId, CancellationToken cancellationToken);
        Task<ICollection<UserToProject>> GetEmployeesAsync(Guid projectId, CancellationToken cancellationToken);
        Task<ICollection<UserToProject>> GetManagersAsync(Guid projectId, CancellationToken cancellationToken);
        Task<Project> GetProjectByIdAsync(GetProjectByIdModel model, CancellationToken cancellationToken);
        Task<ICollection<TagToProject>> GetTagsAsync(Guid projectId, CancellationToken cancellationToken);
        Task<bool> IsExistByConditionAsync(
             Expression<Func<Project, bool>> condition,
             CancellationToken cancellationToken);
        Task DeleteAsync(Guid projectId, CancellationToken cancellationToken);
        Task<ICollection<Project>> GetProjectsByIds(List<Guid> ids, CancellationToken cancellationToken);

        Task<ICollection<Project>> GetAllProjectByUserId(Guid userId, bool includeCreatedProjects = false, CancellationToken cancellationToken = default);
    }
}
