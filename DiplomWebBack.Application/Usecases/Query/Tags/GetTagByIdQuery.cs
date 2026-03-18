using DiplomWebBack.Application.DTOs.Tags.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Application.Usecases.Query.Tags
{
    public record GetTagByIdQuery(Guid Id) : IRequest<TagResponseDto?>;
}
