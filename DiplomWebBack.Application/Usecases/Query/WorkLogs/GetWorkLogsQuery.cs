using DiplomWebBack.Application.DTOs.WorkLogs.Request;
using DiplomWebBack.Application.DTOs.WorkLogs.Response;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Query.WorkLogs
{
    public record GetWorkLogsQuery(GetWorkLogsRequest Request) : IRequest<List<WorkLogMainInfoResponse>>;
}
