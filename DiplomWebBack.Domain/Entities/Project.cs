using DiplomWebBack.Domain.Entities.m2m;

namespace DiplomWebBack.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        public ICollection<UserToProject> UserToProjects { get; set; } = new List<UserToProject>();
    }
}
