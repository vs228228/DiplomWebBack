using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Command.User;
using DiplomWebBack.Domain.Entities.Responses;
using DiplomWebBack.Domain.Interfaces;
using DiplomWebBack.Domain.Repos;
using Mapster;
using MediatR;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.User
{
    public class MatchProjectCommandHandler : IRequestHandler<MatchProjectCommand, List<ProjectMatchResponseDto>>
    {
        private readonly IUserSkillsRepository _userSkillsRepository;
        private readonly IUserVerificationService _userVerificationService;
        private readonly IExternalApiService _externalApiService;
        private readonly IProjectRepository _projectRepository;

        public MatchProjectCommandHandler(
            IUserSkillsRepository userSkillsRepository, 
            IUserVerificationService userVerificationService, 
            IExternalApiService externalApiService,
            IProjectRepository projectRepository)
        {
            _userSkillsRepository = userSkillsRepository;
            _userVerificationService = userVerificationService;
            _externalApiService = externalApiService;
            _projectRepository = projectRepository;
        }

        public async Task<List<ProjectMatchResponseDto>> Handle(MatchProjectCommand request, CancellationToken cancellationToken)
        {
            var initiator = await _userVerificationService.CheckIfUserValidAndGetAsync(request.InitiatorId, cancellationToken);

            var user = await _userVerificationService.CheckIfUserValidAndGetAsync(request.Request.UserId, cancellationToken);

            var skills = await _userSkillsRepository.GetByUserIdAsync(user.Id);

            var projects = await _projectRepository.GetProjectsByIds(request.Request.ProjectIds, cancellationToken);

            var matchingProjects = await _externalApiService.MatchProjectsAsync(skills, projects, cancellationToken);

            matchingProjects = matchingProjects.Where(p => p.Score >= request.Request.MinScorePercent).ToList();

            matchingProjects.OrderBy(p => p.Score);

            return matchingProjects.Adapt<List<ProjectMatchResponseDto>>();
        }
    }
}
