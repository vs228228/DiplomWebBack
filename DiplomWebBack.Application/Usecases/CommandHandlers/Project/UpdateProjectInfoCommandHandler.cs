using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Command.Projects;
using DiplomWebBack.Domain.GetByIdModels;
using DiplomWebBack.Domain.Interfaces;
using DiplomWebBack.Domain.Repos;
using DiplomWebBack.DomainRepos.Repos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.Project
{
    public class UpdateProjectInfoCommandHandler : IRequestHandler<UpdateProjectInfoCommand, Unit>
    {
        private readonly IUserVerificationService _userVerificationService;
        private readonly IProjectVerificationService _projectVerificationService;
        private readonly IProjectRepository _projectRepository;
        private readonly IFileService _fileService;

        public UpdateProjectInfoCommandHandler(IUserVerificationService userVerificationService,
            IProjectVerificationService projectVerificationService,
            IProjectRepository projectRepository,
            IFileService fileService) 
        {
            _projectRepository = projectRepository;
            _userVerificationService = userVerificationService;
            _projectVerificationService = projectVerificationService;
            _fileService = fileService;
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

            if(request.Dto.TechnicalTask is not null)
            {
                var extension = Path.GetExtension(request.Dto.TechnicalTask.FileName);

                var fileName = $"{request.Dto.Title}_{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}{extension}";

                var tz = (await _fileService.SaveFilesAsync(new List<IFormFile> { request.Dto.TechnicalTask }, new List<string> { fileName }))[0];

                project.TechnicalTask = tz;
            }

            await _projectRepository.UpdateAsync(project);

            return Unit.Value;
        }
    }
}
