using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DiplomWebBack.Domain.Entities.Responses
{
    public class UserSkillsDocument
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public Guid UserId { get; set; }

        public List<SkillItem> Skills { get; set; } = new();

        public int TotalFound { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
