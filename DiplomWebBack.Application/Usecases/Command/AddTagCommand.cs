using DiplomWebBack.Application.DTOs.Tags.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Application.Usecases.Command
{
    public record AddTagCommand(string Title) : IRequest<Guid>;
}
