using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Domain.Entities.m2m
{
    
    public class TagToProject
    {
        public Guid ProjectId { get; set; }
        public Project Project {  get; set; }
        
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
