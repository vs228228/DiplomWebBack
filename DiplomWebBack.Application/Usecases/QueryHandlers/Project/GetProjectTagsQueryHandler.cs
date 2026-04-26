using DiplomWebBack.Application.DTOs.Project.Request;
using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Query.Projects;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Repos;
using Mapster;
using MediatR;

namespace DiplomWebBack.Application.Usecases.QueryHandlers.Project
{
    public class GetProjectTagsQueryHandler : IRequestHandler<GetProjectTagsQuery, IEnumerable<ProjectTagRequestDto>>
    {
        private readonly IUserVerificationService _userVerificationService;
        private readonly IProjectRepository _projectRepository;

        public GetProjectTagsQueryHandler(IUserVerificationService userVerificationService, IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
            _userVerificationService = userVerificationService;
        }

        public async Task<IEnumerable<ProjectTagRequestDto>> Handle(GetProjectTagsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userVerificationService.CheckIfUserValidAndGetAsync(request.UserId, cancellationToken);

            var isExis = await _projectRepository.IsExistByConditionAsync(p => p.Id == request.ProjectId, cancellationToken);

            if (!isExis)
            {
                throw new NotFoundException("Проект не найден");
            }

            var tags = await _projectRepository.GetTagsAsync(request.ProjectId, cancellationToken);

            return tags.Adapt<List<ProjectTagRequestDto>>();
        }
    }
}
