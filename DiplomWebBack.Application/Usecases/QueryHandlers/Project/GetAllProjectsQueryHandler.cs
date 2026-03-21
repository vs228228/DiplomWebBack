using DiplomWebBack.Application.DTOs.Project.Response;
using DiplomWebBack.Application.Usecases.Query.Projects;
using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Repos;
using Mapster;
using MediatR;

namespace DiplomWebBack.Application.Usecases.QueryHandlers.Project
{
    public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, PaginatedList<ProjectResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;

        public GetAllProjectsQueryHandler(IUserRepository userRepository, IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public async Task<PaginatedList<ProjectResponseDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectRepository.GetAllPagedAsync(request.PageNumber, request.PageSize, cancellationToken);

            return projects.Adapt<PaginatedList<ProjectResponseDto>>();
        }
    }
}
