using DiplomWebBack.Application.DTOs.Project.Response;
using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Query.Projects;
using DiplomWebBack.Domain.GetByIdModels;
using DiplomWebBack.Domain.Repos;
using Mapster;
using MediatR;

namespace DiplomWebBack.Application.Usecases.QueryHandlers.Project
{
    internal class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectResponseDto>
    {
        private readonly IProjectVereficationService _projectVereficationService;

        public GetProjectByIdQueryHandler(IProjectVereficationService projectVereficationService)
        {
            _projectVereficationService = projectVereficationService;
        }

        public async Task<ProjectResponseDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var model = new GetProjectByIdModel()
            {
                Id = request.Id,
                TrackChanges = false,
            };

            var project = await _projectVereficationService.CheckIfProjectValidForReadAndGetAsync(model, cancellationToken);

            return project.Adapt<ProjectResponseDto>();
        }
    }
}
