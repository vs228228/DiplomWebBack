using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Command.WorkLogs;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Repos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.WorkLogs
{
    public class DeleteWorkLogCommandHandler : IRequestHandler<DeleteWorkLogCommand, Unit>
    {
        private readonly IUserVerificationService _userVerificationService;
        private readonly IWorkLogRepository _workLogRepository;

        public DeleteWorkLogCommandHandler(IUserVerificationService userVerificationService, IWorkLogRepository workLogRepository)
        {
            _userVerificationService = userVerificationService;
            _workLogRepository = workLogRepository;
        }

        public async Task<Unit> Handle(DeleteWorkLogCommand request, CancellationToken cancellationToken)
        {
            var initiator = await _userVerificationService.CheckIfUserValidAndGetAsync(request.InitiatorId, cancellationToken);

            var workLog = await _workLogRepository.GetWorkLogById(request.WorkLogId, cancellationToken);

            if (initiator.Id != workLog.UserId && initiator.Role == Domain.Enums.UserRole.Employee)
            {
                throw new ForbiddenException("Нет прав на создание ворклога этому пользователю");
            }

            await _workLogRepository.DeleteAsync(workLog, cancellationToken);

            return Unit.Value;
        }
    }
}
