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

        public async Task SaveAsync(Guid userId, SkillExtraction response)
        {
            var existing = await _collection
                .Find(x => x.UserId == userId)
                .SortByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            // проставляем Id новым скиллам (если вдруг null/default)
            foreach (var skill in response.Skills)
            {
                if (skill.Id == Guid.Empty)
                    skill.Id = Guid.NewGuid();
            }

            if (existing == null)
            {
                var document = new UserSkillsDocument
                {
                    UserId = userId,
                    Skills = response.Skills
                        .GroupBy(s => s.Name.ToLower())
                        .Select(g => g.First())
                        .ToList(),
                    TotalFound = response.TotalFound,
                    CreatedAt = DateTime.UtcNow
                };

                await _collection.InsertOneAsync(document);

                return;
            }

            // делаем словарь существующих по имени
            var existingDict = existing.Skills
                .GroupBy(s => s.Name.ToLower())
                .ToDictionary(g => g.Key, g => g.First());

            foreach (var newSkill in response.Skills)
            {
                var key = newSkill.Name.ToLower();

                if (!existingDict.ContainsKey(key))
                {
                    existingDict[key] = newSkill; // новый скилл
                }
                else
                {
                    // если хочешь — можно обновлять данные
                    var existingSkill = existingDict[key];
                    existingSkill.Level = newSkill.Level;
                    existingSkill.Years = newSkill.Years;
                }
            }

            var mergedSkills = existingDict.Values.ToList();

            var update = Builders<UserSkillsDocument>.Update
                .Set(x => x.Skills, mergedSkills)
                .Set(x => x.TotalFound, mergedSkills.Count)
                .Set(x => x.CreatedAt, DateTime.UtcNow);

            await _collection.UpdateOneAsync(
                x => x.Id == existing.Id,
                update);
        }

        public async Task<UserSkillsDocument?> GetByUserIdAsync(Guid userId)
        {
            return await _collection
                .Find(x => x.UserId == userId)
                .SortByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task RemoveSkillAsync(Guid userId, Guid skillId)
        {
            // удаляем
            var pullUpdate = Builders<UserSkillsDocument>.Update
                .PullFilter(x => x.Skills, s => s.Id == skillId);

            await _collection.UpdateOneAsync(
                x => x.UserId == userId,
                pullUpdate);

            // получаем обновлённый документ
            var doc = await _collection
                .Find(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (doc != null)
            {
                var updateTotal = Builders<UserSkillsDocument>.Update
                    .Set(x => x.TotalFound, doc.Skills.Count);

                await _collection.UpdateOneAsync(
                    x => x.UserId == userId,
                    updateTotal);
            }
        }
    }
}
