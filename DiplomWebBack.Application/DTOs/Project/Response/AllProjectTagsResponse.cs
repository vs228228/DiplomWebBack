using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Application.DTOs.Project.Response
{
    public class AllProjectTagsResponse
    {
        public string Title { get; set; }
        public IEnumerable<ProjectTagResponseDto> Tags { get; set; }
    }
}
