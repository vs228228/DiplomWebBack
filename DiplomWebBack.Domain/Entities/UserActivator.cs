namespace DiplomWebBack.Domain.Entities
{
    public class UserActivator
    { 
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid? AprovedById {  get; set; }
        public User? AprovedBy { get; set; }
        public DateTimeOffset? AprovedDate { get; set; } = DateTimeOffset.UtcNow;
    }
}
