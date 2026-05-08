using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Application.Usecases.Command.WorkLogs
{
    public record DeleteWorkLogCommand(Guid WorkLogId, Guid InitiatorId) : IRequest<Unit>;
}
