using DiplomWebBack.Domain.Enums;

namespace DiplomWebBack.Domain.Entities.m2m
{
    public class UserToProject
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTimeOffset JoinedAt { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public ProjectRole Role { get; set; }
    }
}
