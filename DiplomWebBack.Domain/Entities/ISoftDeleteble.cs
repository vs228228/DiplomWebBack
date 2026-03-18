namespace DiplomWebBack.Domain.Entities
{
    public interface ISoftDeleteble
    {
        public bool IsDelete { get; set; }
    }
}
