using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Command.WorkLogs;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.GetByIdModels;
using DiplomWebBack.Domain.Repos;
using MediatR;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.WorkLogs
{
    public class CreateWorkLogCommandHandler : IRequestHandler<CreateWorkLogCommand, Guid>
    {
        private IWorkLogRepository _workLogRepository;
        private IUserVerificationService _userVerificationService;
        private IProjectVerificationService _projectVerificationService;

        public CreateWorkLogCommandHandler(IWorkLogRepository workLogRepository, IUserVerificationService userVerificationService, IProjectVerificationService projectVerificationService)
        {
            _workLogRepository = workLogRepository;
            _userVerificationService = userVerificationService;
            _projectVerificationService = projectVerificationService;
        }

        public async Task<Guid> Handle(CreateWorkLogCommand request, CancellationToken cancellationToken)
        {
            var initiator = await _userVerificationService.CheckIfUserValidAndGetAsync(request.UserId, cancellationToken);

            var user = await _userVerificationService.CheckIfUserValidAndGetAsync(request.Request.UserId, cancellationToken);

            if(initiator.Id != user.Id && initiator.Role == Domain.Enums.UserRole.Employee)
            {
                throw new ForbiddenException("Нет прав на создание ворклога этому пользователю");
            }

            var model = new GetProjectByIdModel(){
                Id = request.Request.ProjectId,
            };

            var project = await _projectVerificationService.CheckIfProjectValidForReadAndGetAsync(model, cancellationToken);

            var workLog = new WorkLog()
            {
                ProjectId = request.Request.ProjectId,
                Description = request.Request.Description,
                SpentMinutes = request.Request.SpentMinutes,
                UserId = user.Id,
                WorkDate = request.Request.WorkDate,
                CreatedAt = DateTime.UtcNow,
                Id = Guid.NewGuid(),
            };

            await _workLogRepository.AddAsync(workLog, cancellationToken);

            return workLog.Id;
        }
    }
}
