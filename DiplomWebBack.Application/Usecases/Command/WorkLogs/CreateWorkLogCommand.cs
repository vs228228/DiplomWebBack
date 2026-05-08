using DiplomWebBack.Application.DTOs.WorkLogs.Request;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Command.WorkLogs
{
    public record CreateWorkLogCommand(Guid UserId, CreateWorkLogRequest Request) : IRequest<Guid>;
}
