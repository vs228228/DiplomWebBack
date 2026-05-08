namespace DiplomWebBack.Application.DTOs.WorkLogs.Request
{
    public class CreateWorkLogRequest
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public DateOnly WorkDate { get; set; }
        public string? Description { get; set; } = string.Empty;
        public int SpentMinutes { get; set; }
    }
}
