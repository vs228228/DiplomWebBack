using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.GetByIdModels;

namespace DiplomWebBack.Application.Services.Interfaces
{
    public interface IProjectVerificationService
    {
        Task<Project> CheckIfProjectValidForUpdateAndGetAsync(GetProjectByIdModel model , Guid userId, bool IsAdmin, CancellationToken cancellationToken,bool trackChanges = false);
        Task<bool> CheckIfProjectValidForUpdateAsync(Guid projectId, Guid userId, bool IsAdmin, CancellationToken cancellationToken);
        Task<Project> CheckIfProjectValidForReadAndGetAsync(GetProjectByIdModel model, CancellationToken cancellationToken);
    }
}
