using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Entities.Responses;
using Microsoft.AspNetCore.Http;

namespace DiplomWebBack.Domain.Interfaces
{
    public interface IExternalApiService
    {
        Task<SkillExtraction> ExtractSkillsAsync(
        IFormFile file,
        CancellationToken ct);
        Task<ResumeAnalysisResponse> AnalyzeResumeAsync(string text, CancellationToken ct);
        Task<List<ProjectMatchResponse>> MatchProjectsAsync(UserSkillsDocument userSkills, IEnumerable<Project> projects, CancellationToken ct);
    }
}
