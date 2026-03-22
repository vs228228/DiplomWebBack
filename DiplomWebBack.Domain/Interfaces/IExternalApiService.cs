using DiplomWebBack.Domain.Entities.Responses;
using Microsoft.AspNetCore.Http;

namespace DiplomWebBack.Domain.Interfaces
{
    public interface IExternalApiService
    {
        Task<SkillExtractionResponse> ExtractSkillsAsync(
        IFormFile file,
        CancellationToken ct);
        Task<ResumeAnalysisResponse> AnalyzeResumeAsync(string text, CancellationToken ct);
        Task<SuggestionsResponse> GetSuggestionsAsync(string skill, CancellationToken ct);
    }
}
