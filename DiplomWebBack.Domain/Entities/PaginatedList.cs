using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Domain.Entities
{
    public class PaginatedList<T>
    {
        public ICollection<T> List { get; set; }
        public MetaForPaginatedList Meta { get; set; }
    }
}
