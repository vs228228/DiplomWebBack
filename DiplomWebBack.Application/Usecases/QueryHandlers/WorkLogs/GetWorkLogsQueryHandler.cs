using DiplomWebBack.Application.DTOs.WorkLogs.Response;
using DiplomWebBack.Application.Usecases.Query.WorkLogs;
using DiplomWebBack.Domain.Repos;
using Mapster;
using MediatR;

namespace DiplomWebBack.Application.Usecases.QueryHandlers.WorkLogs
{
    public class GetWorkLogsQueryHandler : IRequestHandler<GetWorkLogsQuery, List<WorkLogMainInfoResponse>>
    {
        private readonly IWorkLogRepository _workLogRepository;

        public GetWorkLogsQueryHandler(IWorkLogRepository workLogRepository)
        {
            _workLogRepository = workLogRepository;
        }

        public async Task<List<WorkLogMainInfoResponse>> Handle(GetWorkLogsQuery request, CancellationToken cancellationToken)
        {
            var workLogs = await _workLogRepository.GetWorkLogs(
                request.Request.StartDate,
                request.Request.EndDate,
                request.Request.UserId,
                request.Request.ProjectId);

            return workLogs.Adapt<List<WorkLogMainInfoResponse>>();
        }
    }
}
