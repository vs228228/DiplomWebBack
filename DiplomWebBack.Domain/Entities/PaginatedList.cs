namespace DiplomWebBack.Domain.Entities
{
    public class PaginatedList<T>
    {
        public ICollection<T> List { get; set; }
        public MetaForPaginatedList Meta { get; set; }
    }
}
