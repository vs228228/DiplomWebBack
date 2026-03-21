using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.GetByIdModels;

namespace DiplomWebBack.Application.Services.Interfaces
{
    public interface IProjectVereficationService
    {
        Task<Project> CheckIfProjectValidForUpdateAndGetAsync(Guid projectId, Guid userId, bool IsAdmin, CancellationToken cancellationToken);
        Task<Project> CheckIfProjectValidForReadAndGetAsync(GetProjectByIdModel model, CancellationToken cancellationToken);
    }
}
