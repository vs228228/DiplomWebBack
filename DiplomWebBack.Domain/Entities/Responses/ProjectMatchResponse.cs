using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiplomWebBack.Domain.Entities.Responses
{
    public class ProjectMatchResponse
    {
        [JsonPropertyName("project_id")]
        public Guid ProjectId { get; set; }

        [JsonPropertyName("project_name")]
        public string ProjectName { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("reason")]
        public List<string> Reason { get; set; }
    }
}
