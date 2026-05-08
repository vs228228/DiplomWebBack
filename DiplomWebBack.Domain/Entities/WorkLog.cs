namespace DiplomWebBack.Domain.Entities
{
    public class WorkLog
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public DateOnly WorkDate { get; set; }
        public string? Description { get; set; } = string.Empty;
        public int SpentMinutes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
