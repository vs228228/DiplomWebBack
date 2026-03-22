using DiplomWebBack.Domain.Entities.Responses;

namespace DiplomWebBack.Domain.Repos
{
    public interface IUserSkillsRepository
    {
        Task SaveAsync(Guid userId, SkillExtractionResponse response);
        Task<UserSkillsDocument?> GetByUserIdAsync(Guid userId);
    }
}
