namespace DiplomWebBack.Application.DTOs.Project.Request
{
    public class ProjectTagRequestDto
    {
        public Guid Id { get; set; }
        public double Year { get; set; } = 0.0;
        public double Weight { get; set; } = 0.0;
    }
}
