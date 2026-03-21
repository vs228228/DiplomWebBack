using DiplomWebBack.Application.Usecases.Command.Projects;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Entities.m2m;
using DiplomWebBack.Domain.Repos;
using DiplomWebBack.DomainRepos.Repos;
using MediatR;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.Project
{
    internal class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITagsRepository _tagsRepository;

        public CreateProjectCommandHandler(IUserRepository userRepository, IProjectRepository projectRepository, ITagsRepository tagsRepository)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _tagsRepository = tagsRepository;
        }

        public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if(user is null)
            {
                throw new BadRequestException("Инициатор не найден");
            }

            var tags = await _tagsRepository.GetByIdsAsync(request.Project.Tags, cancellationToken);

            var project = new Domain.Entities.Project()
            {
                CreatedAt = DateTime.UtcNow,
                CreatedById = user.Id,
                Description = request.Project.Description,
                Title = request.Project.Title,
                Tags = tags,

            };

            await _projectRepository.AddAsync(project, cancellationToken);

            return project.Id;
        }
    }
}
