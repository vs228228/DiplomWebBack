using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace DiplomWebBack.Domain.Entities.Responses
{
    public class SkillExtraction
    {
        [JsonPropertyName("skills")]
        public List<SkillItem> Skills { get; set; } = new();

        [JsonPropertyName("total_found")]
        public int TotalFound { get; set; }
    }

    public class SkillItem
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("level")]
        public string? Level { get; set; }

        [JsonPropertyName("years")]
        public double? Years { get; set; }
    }
}