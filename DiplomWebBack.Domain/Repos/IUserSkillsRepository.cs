using DiplomWebBack.Domain.Entities.Responses;

namespace DiplomWebBack.Domain.Repos
{
    public interface IUserSkillsRepository
    {
        Task SaveAsync(Guid userId, SkillExtraction response);
        Task<UserSkillsDocument?> GetByUserIdAsync(Guid userId, string searchBy = "");
        Task RemoveSkillAsync(Guid userId, Guid skillId);
    }
}
