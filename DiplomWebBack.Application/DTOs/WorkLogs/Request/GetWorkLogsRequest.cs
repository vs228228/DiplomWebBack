namespace DiplomWebBack.Application.DTOs.WorkLogs.Request
{
    public record GetWorkLogsRequest(DateOnly? StartDate = null, DateOnly? EndDate = null, Guid? UserId = null, Guid? ProjectId = null);
}
