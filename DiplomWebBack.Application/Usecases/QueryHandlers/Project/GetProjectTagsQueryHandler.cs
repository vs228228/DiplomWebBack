using DiplomWebBack.Application.DTOs.Project.Request;
using DiplomWebBack.Application.DTOs.Project.Response;
using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Query.Projects;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.GetByIdModels;
using DiplomWebBack.Domain.Repos;
using Mapster;
using MediatR;

namespace DiplomWebBack.Application.Usecases.QueryHandlers.Project
{
    public class GetProjectTagsQueryHandler : IRequestHandler<GetProjectTagsQuery, AllProjectTagsResponse>
    {
        private readonly IUserVerificationService _userVerificationService;
        private readonly IProjectRepository _projectRepository;

        public GetProjectTagsQueryHandler(IUserVerificationService userVerificationService, IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
            _userVerificationService = userVerificationService;
        }

        public async Task<AllProjectTagsResponse> Handle(GetProjectTagsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userVerificationService.CheckIfUserValidAndGetAsync(request.UserId, cancellationToken);

            var isExis = await _projectRepository.IsExistByConditionAsync(p => p.Id == request.ProjectId, cancellationToken);

            if (!isExis)
            {
                throw new NotFoundException("Проект не найден");
            }

            var project = await _projectRepository.GetProjectByIdAsync(new GetProjectByIdModel
            {
                Id = request.ProjectId,
                IncludeTags = true,
                IncludeEmployee = false,
                IncludeCreatedBy = false,
                TrackChanges = false
            }, cancellationToken);

            var tags = project.ProjectTags.Select(t => new ProjectTagResponseDto
            {
                Id = t.TagId,
                Year = t.Year,
                Weight = t.Weight,
                Title = t.Tag.Title
            });

            return new AllProjectTagsResponse(){ Title = project.Title, Tags = tags };
        }
    }
}
