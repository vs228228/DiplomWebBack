using DiplomWebBack.Application.Usecases.Command.Projects;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Entities.m2m;
using DiplomWebBack.Domain.Interfaces;
using DiplomWebBack.Domain.Repos;
using DiplomWebBack.DomainRepos.Repos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.Project
{
    internal class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITagsRepository _tagsRepository;
        private readonly IFileService _fileService;

        public CreateProjectCommandHandler(
            IUserRepository userRepository, 
            IProjectRepository projectRepository,
            ITagsRepository tagsRepository,
            IFileService fileService)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _tagsRepository = tagsRepository;
            _fileService = fileService;
        }

        public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if(user is null)
            {
                throw new BadRequestException("Инициатор не найден");
            }

           /* List<Guid> usersIds = new List<Guid>();

            foreach(var projectUser in request.Project.Users)
            {
                usersIds.Add(projectUser.Id);
            }

            if(!await _userRepository.AreAllUsersExistAsync(usersIds))
            {
                throw new BadRequestException("Не все пользователи существуют");
            }*/ // закомментил т.к. Женька то просит добавить то просит удалить, так что фиг его знает, вдруг снова понадобится

            var tags = await _tagsRepository.GetByIdsAsync(request.Project.Tags.Select(t => t.Id).ToList(), cancellationToken);

            /*var userToProjects = request.Project.Users
                .Select(u => new UserToProject
                {
                    UserId = u.Id,
                    JoinedAt = DateTime.UtcNow,
                    Role = u.Role
                })
                .ToList();

            if (!userToProjects.Any(u => u.UserId == user.Id))
            {
                userToProjects.Add(new UserToProject
                {
                    UserId = user.Id,
                    JoinedAt = DateTime.UtcNow,
                    Role = ProjectRole.Manager
                });
            }*/ // такая же ситуация

            string tz = "";

            if (request.Project.TechnicalTask is not null)
            {

                var extension = Path.GetExtension(request.Project.TechnicalTask.FileName);

                var fileName = $"{request.Project.Title}_{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}{extension}";

                tz = (await _fileService.SaveFilesAsync(new List<IFormFile> { request.Project.TechnicalTask }, new List<string> { fileName }))[0];
            }

            var project = new Domain.Entities.Project()
            {
                CreatedAt = DateTime.UtcNow,
                CreatedById = user.Id,
                Description = request.Project.Description,
                Title = request.Project.Title,
                ProjectTags = new List<TagToProject>(),
                Customer = request.Project.Customer,
                TechnicalTask = tz,

            };

            foreach (var tag in request.Project.Tags)
            {
                project.ProjectTags.Add(new TagToProject() { TagId = tag.Id, Weight = tag.Weight, Year = tag.Year });
            }

            await _projectRepository.AddAsync(project, cancellationToken);

            return project.Id;
        }
    }
}
