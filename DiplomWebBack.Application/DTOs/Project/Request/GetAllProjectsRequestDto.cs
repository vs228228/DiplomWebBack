using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Application.DTOs.Project.Request
{
    public record class GetAllProjectsRequestDto(
        int PageNumber = 1,
        int PageSize = 10,
        string SearchBy = "",
        IEnumerable<Guid>? FilterByCreator = null!, 
        IEnumerable<Guid>? FiltredByTags = null!);
}
