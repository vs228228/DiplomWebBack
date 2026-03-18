using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Application.Usecases.Command.UserActivator
{
    public record class ActivateUserCommand(Guid UserId, Guid InitiatorId) : IRequest<Unit>;
}
