using DiplomWebBack.Application.DTOs.UserActivator.Response;
using DiplomWebBack.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Application.Usecases.Query.UserActivator
{
    public record GetAllUserActivatorsQuery(int pageNumber, int pageSize, Guid UserId) : IRequest<PaginatedList<UserActivatorResponse>>;
}
