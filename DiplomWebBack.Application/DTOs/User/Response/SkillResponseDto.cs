using DiplomWebBack.Domain.Entities.Responses;

namespace DiplomWebBack.Application.DTOs.User.Response
{
    public class SkillResponseDto
    {
        public List<SkillItem> Skills { get; set; } = new();
        public int TotalFound { get; set; }
    }
}
