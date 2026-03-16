using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Application.DTOs.Tags.Responses
{
    public class TagResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;
    }
}
