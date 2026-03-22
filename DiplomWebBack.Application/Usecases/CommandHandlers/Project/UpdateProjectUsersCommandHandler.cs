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
            var user = await _userVerificationService.CheckIfUserValidAndGetAsync(request.UserId, cancellationToken);

            var model = new GetProjectByIdModel()
            {
                Id = request.ProjectId,
                TrackChanges = true,
                IncludeEmployee = true,
            };

            var project = await _projectVerificationService.CheckIfProjectValidForUpdateAndGetAsync(model, user.Id,
                user.Role == Domain.Enums.UserRole.Admin ? true : false, cancellationToken, trackChanges: true);

            project.UserToProjects.Clear();

            await _userRepository.RemoveUserToProjectAsync(project.Id);

            var users = request.Dto.Users.DistinctBy(u => u.Id).ToList();

            foreach (var userr in users)
            {
                project.UserToProjects.Add(new UserToProject() { JoinedAt = DateTime.UtcNow, ProjectId = project.Id, UserId =  userr.Id, Role = userr.Role});
            }

            await _projectRepository.UpdateAsync(project);

            return Unit.Value;
        }
    }
}

