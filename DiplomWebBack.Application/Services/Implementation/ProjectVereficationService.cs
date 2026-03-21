using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.GetByIdModels;
using DiplomWebBack.Domain.Repos;

namespace DiplomWebBack.Application.Services.Implementation
{
    public class ProjectVereficationService : IProjectVereficationService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public ProjectVereficationService(IProjectRepository projectRepository) 
        {
            _projectRepository = projectRepository;
        }

        public async Task<Project> CheckIfProjectValidForReadAndGetAsync(GetProjectByIdModel model, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetProjectByIdAsync(model, cancellationToken);

            if(project is null)
            {
                throw new NotFoundException("Проект не найден");
            }

            return project;
        }

        public async Task<Project> CheckIfProjectValidForUpdateAndGetAsync(Guid projectId, Guid userId, bool IsAdmin, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
