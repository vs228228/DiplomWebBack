using Microsoft.AspNetCore.Http;

namespace DiplomWebBack.Application.DTOs.Project.Request
{
    public class ProjectCreateRequestDto
    { 
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
       /* public IEnumerable<UserProjectCreateRequestDto> Users { get; set; }*/
       public string Customer { get; set; }
        public IFormFile? TechnicalTask { get; set; } = null;
    }
}