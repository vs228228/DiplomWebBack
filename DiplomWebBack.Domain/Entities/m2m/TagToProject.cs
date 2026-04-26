namespace DiplomWebBack.Domain.Entities.m2m
{
    
    public class TagToProject
    {
        public Guid ProjectId { get; set; }
        public Project Project {  get; set; }
        
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }

        public double Year { get; set; }
        public double Weight { get; set; }
    }
}
