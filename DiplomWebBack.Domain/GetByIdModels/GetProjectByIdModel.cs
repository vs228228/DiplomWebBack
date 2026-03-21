namespace DiplomWebBack.Domain.GetByIdModels
{
    public class GetProjectByIdModel
    {
        public Guid Id { get; set; }
        public bool TrackChanges { get; set; } = false;
        public bool IncludeEmployee { get; set; } = false;
        public bool IncludeTags { get; set; } = false;
        public bool IncludeCreatedBy { get; set; } = false;
    }
}