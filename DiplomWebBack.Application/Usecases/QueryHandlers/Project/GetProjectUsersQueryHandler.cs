using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Query.Projects;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Repos;
using Mapster;
using MediatR;

namespace DiplomWebBack.Application.Usecases.QueryHandlers.Project
{
    public class GetProjectUsersQueryHandler : IRequestHandler<GetProjectUsersQuery, IEnumerable<UserProjectResponseDto>>
    {
        private readonly IUserVerificationService _userVerificationService;
        private readonly IProjectRepository _projectRepository;

        public GetProjectUsersQueryHandler(IUserVerificationService userVerificationService, IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
            _userVerificationService = userVerificationService;
        }

        public async Task<IEnumerable<UserProjectResponseDto>> Handle(GetProjectUsersQuery request, CancellationToken cancellationToken)
        {
            var user = await _userVerificationService.CheckIfUserValidAndGetAsync(request.UserId, cancellationToken);

            var isExis = await _projectRepository.IsExistByConditionAsync(p => p.Id == request.ProjectId, cancellationToken);

            if (!isExis)
            {
                throw new NotFoundException("Проект не найден");
            }

            var users = await _projectRepository.GetUsersAsync(request.ProjectId, cancellationToken);

            return users.Adapt<IEnumerable<UserProjectResponseDto>>();
        }
    }
}
