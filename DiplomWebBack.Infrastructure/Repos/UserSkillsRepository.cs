using DiplomWebBack.Domain.Entities.Responses;
using DiplomWebBack.Domain.Repos;
using MongoDB.Driver;

namespace DiplomWebBack.Infrastructure.Repos
{
    public class UserSkillsRepository : IUserSkillsRepository
    {
        private readonly IMongoCollection<UserSkillsDocument> _collection;

        public UserSkillsRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<UserSkillsDocument>("user_skills");
        }

        public async Task SaveAsync(Guid userId, SkillExtractionResponse response)
        {
            var document = new UserSkillsDocument
            {
                UserId = userId,
                Skills = response.Skills,
                TotalFound = response.TotalFound,
                CreatedAt = DateTime.UtcNow
            };

            await _collection.InsertOneAsync(document);
        }

        public async Task<UserSkillsDocument?> GetByUserIdAsync(Guid userId)
        {
            return await _collection
                .Find(x => x.UserId == userId)
                .SortByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
