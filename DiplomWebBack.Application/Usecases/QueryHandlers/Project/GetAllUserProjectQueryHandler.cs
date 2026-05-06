using DiplomWebBack.Application.DTOs.Project.Response;
using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Query.Projects;
using DiplomWebBack.Domain.Repos;
using Mapster;
using MediatR;

namespace DiplomWebBack.Application.Usecases.QueryHandlers.Project
{
    public class GetAllUserProjectQueryHandler : IRequestHandler<GetAllUserProjectQuery, ICollection<ProjectResponseDto>>
    {
        private readonly IUserVerificationService _userVerificationService;
        private readonly IProjectRepository _projectRepository;

        public GetAllUserProjectQueryHandler(IUserVerificationService userVerificationService, IProjectRepository projectRepository)
        {
            _userVerificationService = userVerificationService;
            _projectRepository = projectRepository;
        }

        public async Task<ICollection<ProjectResponseDto>> Handle(GetAllUserProjectQuery request, CancellationToken cancellationToken)
        {
            var user = await _userVerificationService.CheckIfUserValidAndGetAsync(request.UserId, cancellationToken);

            var projects = await _projectRepository.GetAllProjectByUserId(user.Id, request.IncludeCreatetProjects, cancellationToken);

            return projects.Adapt<ICollection<ProjectResponseDto>>();
        }
    }
}
