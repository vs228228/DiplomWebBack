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
        private readonly IProjectVerificationService _projectVereficationService;
        private readonly IUserVerificationService _userVerificationService;

        public GetProjectByIdQueryHandler(IProjectVerificationService projectVereficationService, IUserVerificationService userVerificationService)
        {
            _projectVereficationService = projectVereficationService;
            _userVerificationService = userVerificationService;
        }

        public async Task<ProjectResponseDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userVerificationService.CheckIfUserValidAndGetAsync(request.UserId, cancellationToken);

            var model = new GetProjectByIdModel()
            {
                Id = request.Id,
                TrackChanges = false,
                IncludeCreatedBy = true,
                IncludeTags = true,
                IncludeEmployee = true
            };

            var project = await _projectVereficationService.CheckIfProjectValidForReadAndGetAsync(model, cancellationToken);

            var response =  project.Adapt<ProjectResponseDto>();

            response.CanEdit = await _projectVereficationService.CheckIfProjectValidForUpdateAsync(project.Id, request.UserId,
                user.Role == Domain.Enums.UserRole.Admin ? true : false, cancellationToken);

            return response;
        }
    }
}
