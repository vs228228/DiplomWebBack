using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Command.Projects;
using DiplomWebBack.Domain.Entities.m2m;
using DiplomWebBack.Domain.GetByIdModels;
using DiplomWebBack.Domain.Repos;
using DiplomWebBack.DomainRepos.Repos;
using MediatR;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.Project
{
    public class UpdateProjectUsersCommandHandler : IRequestHandler<UpdateProjectUsersCommand, Unit>
    {
        private readonly IUserVerificationService _userVerificationService;
        private readonly IProjectVerificationService _projectVerificationService;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public UpdateProjectUsersCommandHandler(IUserVerificationService userVerificationService,
            IProjectVerificationService projectVerificationService,
            IProjectRepository projectRepository,
            IUserRepository tagsRepository)
        {
            _userVerificationService = userVerificationService;
            _projectVerificationService = projectVerificationService;
            _projectRepository = projectRepository;
            _userRepository = tagsRepository;
        }

        public async Task<Unit> Handle(UpdateProjectUsersCommand request, CancellationToken cancellationToken)
        {
            var user = await _userVerificationService
                .CheckIfUserValidAndGetAsync(request.UserId, cancellationToken);

            var model = new GetProjectByIdModel()
            {
                Id = request.ProjectId,
                TrackChanges = true,
                IncludeEmployee = true,
            };

            var project = await _projectVerificationService
                .CheckIfProjectValidForUpdateAndGetAsync(
                    model,
                    user.Id,
                    user.Role == Domain.Enums.UserRole.Admin,
                    cancellationToken,
                    trackChanges: true);

            var existingUsers = project.UserToProjects.ToList();

            var newUsers = request.Dto.Users
                .DistinctBy(u => u.Id)
                .ToList();

            var existingUserIds = existingUsers.Select(x => x.UserId).ToHashSet();
            var newUserIds = newUsers.Select(x => x.Id).ToHashSet();

            var usersToRemove = existingUsers
                .Where(x => !newUserIds.Contains(x.UserId))
                .ToList();

            foreach (var userToRemove in usersToRemove)
            {
                project.UserToProjects.Remove(userToRemove);
            }

            var usersToAdd = newUsers
                .Where(x => !existingUserIds.Contains(x.Id))
                .ToList();

            foreach (var userToAdd in usersToAdd)
            {
                project.UserToProjects.Add(new UserToProject
                {
                    ProjectId = project.Id,
                    UserId = userToAdd.Id,
                    Role = userToAdd.Role,
                    JoinedAt = DateTime.UtcNow
                });
            }

            await _projectRepository.UpdateAsync(project);

            return Unit.Value;
        }
    }
}

