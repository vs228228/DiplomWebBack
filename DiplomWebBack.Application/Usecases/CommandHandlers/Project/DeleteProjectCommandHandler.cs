using DiplomWebBack.Application.Services.Implementation;
using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Command.Projects;
using DiplomWebBack.Domain.Repos;
using MediatR;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.Project
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Unit>
    {
        private readonly IUserVerificationService _userVerificationService;
        private readonly IProjectVerificationService _projectVerificationService;
        private readonly IProjectRepository _projectRepository;

        public DeleteProjectCommandHandler(
            IUserVerificationService userVerificationService,
            IProjectVerificationService projectVerificationService,
            IProjectRepository projectRepository) 
        {
            _projectRepository = projectRepository;
            _userVerificationService = userVerificationService;
            _projectVerificationService = projectVerificationService;
        }

        public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var user = await _userVerificationService.CheckIfUserValidAndGetAsync(request.UserId, cancellationToken);

            await _projectVerificationService.CheckIfProjectValidForUpdateAsync(
                request.ProjectId, 
                request.UserId, 
                user.Role == Domain.Enums.UserRole.Admin ? true : false, 
                cancellationToken);

            await _projectRepository.DeleteAsync(request.ProjectId, cancellationToken);

            return Unit.Value;
        }
    }
}
