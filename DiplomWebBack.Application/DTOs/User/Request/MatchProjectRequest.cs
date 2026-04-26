namespace DiplomWebBack.Application.DTOs.User.Request
{
    public class MatchProjectsRequestDto
    {
        public Guid UserId { get; set; }
        public List<Guid> ProjectIds { get; set; }
        public double MinScorePercent { get; set; } = 0.0;
    }
}
