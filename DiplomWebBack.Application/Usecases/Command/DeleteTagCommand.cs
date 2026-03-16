using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Application.Usecases.Command
{
    public record DeleteTagCommand(Guid Id) : IRequest;
}
