using DiplomWebBack.Application.DTOs.Project.Request;
using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Domain.Entities;

namespace DiplomWebBack.Application.DTOs.Project.Response
{
    public class ProjectResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Customer { get; set; }

        public DateTime CreatedAt { get; set; }

        public UserProfileDto CreatedBy { get; set; }

        public ICollection<ProjectTagResponseDto> Tags { get; set; } = new List<ProjectTagResponseDto>();

        public ICollection<UserProjectResponseDto> Users { get; set; } = new List<UserProjectResponseDto>();
        public bool CanEdit { get; set; }
        public string TechnicalTask { get; set; }
    }
}
