using DiplomWebBack.Domain.Entities.m2m;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomWebBack.Domain.Entities
{
    public class Project : ISoftDeleteble
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Customer { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }

        public ICollection<TagToProject> ProjectTags { get; set; } = new List<TagToProject>();

        public ICollection<UserToProject> UserToProjects { get; set; } = new List<UserToProject>();
        public bool IsDelete { get; set; }

        public string TechnicalTask { get; set; }
    }
}
