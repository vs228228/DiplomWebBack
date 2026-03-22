using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Command.Projects;
using DiplomWebBack.Domain.Entities.m2m;
using DiplomWebBack.Domain.GetByIdModels;
using DiplomWebBack.Domain.Repos;
using DiplomWebBack.DomainRepos.Repos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.Project
{
    public class UpdateProjectTagsCommandHandler : IRequestHandler<UpdateProjectTagsCommand, Unit>
    {
        private readonly IUserVerificationService _userVerificationService;
        private readonly IProjectVerificationService _projectVerificationService;
        private readonly IProjectRepository _projectRepository;
        private readonly ITagsRepository _tagsRepository;

        public UpdateProjectTagsCommandHandler(IUserVerificationService userVerificationService,
            IProjectVerificationService projectVerificationService, 
            IProjectRepository projectRepository,
            ITagsRepository tagsRepository)
        {
            _userVerificationService = userVerificationService;
            _projectVerificationService = projectVerificationService;
            _projectRepository = projectRepository;
            _tagsRepository = tagsRepository;
        }

        public async Task<Unit> Handle(UpdateProjectTagsCommand request, CancellationToken cancellationToken)
        {
            var user = await _userVerificationService.CheckIfUserValidAndGetAsync(request.UserId, cancellationToken);

            var model = new GetProjectByIdModel()
            {
                Id = request.ProjectId,
                TrackChanges = true,
            };

            var project = await _projectVerificationService.CheckIfProjectValidForUpdateAndGetAsync(model, user.Id,
                user.Role == Domain.Enums.UserRole.Admin ? true : false, cancellationToken, trackChanges: true);

            project.ProjectTags.Clear();

            await _tagsRepository.RemoveProjectTagsAsync(project.Id);

            var tags = request.Dto.Tags.DistinctBy(t => t).ToList();

            foreach (var tag in tags)
            {
                project.ProjectTags.Add(new TagToProject() { TagId = tag, ProjectId = project.Id });
            }

            await _projectRepository.UpdateAsync(project);

            return Unit.Value;
        }
    }
}
