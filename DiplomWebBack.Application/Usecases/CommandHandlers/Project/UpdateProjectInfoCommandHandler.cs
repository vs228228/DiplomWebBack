using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Command.Projects;
using DiplomWebBack.Domain.GetByIdModels;
using DiplomWebBack.Domain.Repos;
using DiplomWebBack.DomainRepos.Repos;
using MediatR;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.Project
{
    public class UpdateProjectInfoCommandHandler : IRequestHandler<UpdateProjectInfoCommand, Unit>
    {
        private readonly IUserVerificationService _userVerificationService;
        private readonly IProjectVerificationService _projectVerificationService;
        private readonly IProjectRepository _projectRepository;

        public UpdateProjectInfoCommandHandler(IUserVerificationService userVerificationService,
            IProjectVerificationService projectVerificationService,
            IProjectRepository projectRepository) 
        {
            _projectRepository = projectRepository;
            _userVerificationService = userVerificationService;
            _projectVerificationService = projectVerificationService;
        }

        public async Task<Unit> Handle(UpdateProjectInfoCommand request, CancellationToken cancellationToken)
        {
            var user = await _userVerificationService.CheckIfUserValidAndGetAsync(request.UserId, cancellationToken);

            var model = new GetProjectByIdModel()
            {
                Id = request.ProjectId,
                IncludeTags = true,
                TrackChanges = true,
            };

            var project = await _projectVerificationService.CheckIfProjectValidForUpdateAndGetAsync(model, user.Id,
                user.Role == Domain.Enums.UserRole.Admin ? true : false, cancellationToken);

            project.Title = request.Dto.Title;
            project.Description = request.Dto.Description;

            await _projectRepository.UpdateAsync(project);

            return Unit.Value;
        }
    }
}
