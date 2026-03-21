using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.GetByIdModels;
using DiplomWebBack.Domain.Repos;

namespace DiplomWebBack.Application.Services.Implementation
{
    public class ProjectVerificationService : IProjectVerificationService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public ProjectVerificationService(IProjectRepository projectRepository) 
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

        public async Task<Project> CheckIfProjectValidForUpdateAndGetAsync(GetProjectByIdModel model, Guid userId, bool IsAdmin, CancellationToken cancellationToken, bool trackChanges = false)
        {
            if (!await _projectRepository.IsExistByConditionAsync(p => p.Id == model.Id, cancellationToken))
            {
                throw new NotFoundException("Проект не найден");
            }

            var isCanEdit = await _projectRepository.IsExistByConditionAsync(p => p.Id == model.Id
            && (IsAdmin
            || p.CreatedById == userId
            || p.UserToProjects.Any(up => up.UserId == userId && up.Role == Domain.Enums.ProjectRole.Manager)), cancellationToken);

            if (!isCanEdit)
            {
                throw new ForbiddenException("Недостаточно прав для редактирования проекта");
            }

            return await _projectRepository.GetProjectByIdAsync(model, cancellationToken);
        }

        public async Task<bool> CheckIfProjectValidForUpdateAsync(Guid projectId, Guid userId, bool IsAdmin, CancellationToken cancellationToken)
        {
            return await _projectRepository.IsExistByConditionAsync(p => p.Id == projectId
            && (IsAdmin
            || p.CreatedById == userId
            || p.UserToProjects.Any(up => up.UserId == userId && up.Role == Domain.Enums.ProjectRole.Manager)), cancellationToken);
        }
    }
}
