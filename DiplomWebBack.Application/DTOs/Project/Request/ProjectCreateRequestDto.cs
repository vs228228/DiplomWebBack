using DiplomWebBack.Domain.Entities;

namespace DiplomWebBack.Application.DTOs.Project.Request
{
    public class ProjectCreateRequestDto
    { 
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
    }
}