using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Infrastructure.Settings
{
    public class ExternalApiOptions
    {
        public string BaseUrl { get; set; }
        public EndpointsOptions Endpoints { get; set; }
    }

    public class EndpointsOptions
    {
        public string ExtractSkills { get; set; }
        public string AnalyzeResume { get; set; }
        public string MatchProjects { get; set; }
    }
}
