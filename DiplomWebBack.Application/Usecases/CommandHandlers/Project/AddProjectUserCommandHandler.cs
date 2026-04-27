using DiplomWebBack.Application.DTOs.Project.Request;
using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Command.Projects;
using DiplomWebBack.Domain.Entities.m2m;
using DiplomWebBack.Domain.Enums;
using DiplomWebBack.Domain.GetByIdModels;
using DiplomWebBack.Domain.Repos;
using MediatR;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.Project
{
        public class AddProjectUserCommandHandler : IRequestHandler<AddProjectUserCommand, Unit>
        {
            private readonly IUserVerificationService _userVerificationService;
            private readonly IProjectVerificationService _projectVerificationService;
            private readonly IProjectRepository _projectRepository;
            private readonly IUserRepository _userRepository;

            public AddProjectUserCommandHandler(
                IUserVerificationService userVerificationService,
                IProjectVerificationService projectVerificationService,
                IProjectRepository projectRepository,
                IUserRepository userRepository)
            {
                _userVerificationService = userVerificationService;
                _projectVerificationService = projectVerificationService;
                _projectRepository = projectRepository;
                _userRepository = userRepository;
            }

            public async Task<Unit> Handle(AddProjectUserCommand request, CancellationToken cancellationToken)
            {
                var initiator = await _userVerificationService
                    .CheckIfUserValidAndGetAsync(request.UserId, cancellationToken);

                var model = new GetProjectByIdModel
                {
                    Id = request.ProjectId,
                    TrackChanges = true,
                    IncludeEmployee = true
                };

                var project = await _projectVerificationService
                    .CheckIfProjectValidForUpdateAndGetAsync(
                        model,
                        initiator.Id,
                        initiator.Role == UserRole.Admin,
                        cancellationToken,
                        trackChanges: true);

                // если пользователь уже в проекте — обновим роль, иначе добавим
                var existing = project.UserToProjects.FirstOrDefault(u => u.UserId == request.Dto.User.Id);

                if (existing == null)
                {
                    project.UserToProjects.Add(new UserToProject
                    {
                        ProjectId = project.Id,
                        UserId = request.Dto.User.Id,
                        Role = request.Dto.User.Role,
                        JoinedAt = DateTime.UtcNow
                    });
                }
                else
                {
                    existing.Role = request.Dto.User.Role;
                }

                await _projectRepository.UpdateAsync(project);

                return Unit.Value;
            }
        }
}
