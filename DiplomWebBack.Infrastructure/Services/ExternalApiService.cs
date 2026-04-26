using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Entities.Responses;
using DiplomWebBack.Domain.Interfaces;
using DiplomWebBack.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DiplomWebBack.Infrastructure.Services
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ExternalApiOptions _options;

        public ExternalApiService(
            HttpClient httpClient,
            IOptions<ExternalApiOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<SkillExtraction> ExtractSkillsAsync(
        IFormFile file,
        CancellationToken ct)
        {
            using var content = new MultipartFormDataContent();

            using var fileStream = file.OpenReadStream();
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(GetContentType(file.FileName));

            content.Add(fileContent, "file", file.FileName);

            var response = await _httpClient.PostAsync(_options.Endpoints.ExtractSkills, content, ct);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<SkillExtraction>(cancellationToken: ct);
        }

        private string GetContentType(string fileName)
        {
            return Path.GetExtension(fileName).ToLower() switch
            {
                ".pdf" => "application/pdf",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };
        }

        public async Task<ResumeAnalysisResponse> AnalyzeResumeAsync(string text, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync(
                _options.Endpoints.AnalyzeResume,
                new { text },
                ct);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ResumeAnalysisResponse>(cancellationToken: ct);
        }

        public async Task<List<ProjectMatchResponse>> MatchProjectsAsync(UserSkillsDocument userSkills, IEnumerable<Project> projects, CancellationToken ct)
        {
            projects = new List<Project> { projects.FirstOrDefault() };

            // 1. Формируем request
            var request = new
            {
                candidate = new
                {
                    skills = userSkills.Skills.Select(s => new
                    {
                        name = s.Name,
                        years = s.Years
                    })
                },
                projects = projects.Select(p => new
                {
                    id = p.Id.ToString(),
                    name = p.Title,
                    skills = p.ProjectTags.Select(pt => new
                    {
                        name = pt.Tag.Title,
                        required_years = pt.Year,
                        weight = pt.Weight
                    })
                })
            };

            // 2. Запрос во внешний API
            var response = await _httpClient.PostAsJsonAsync(
                _options.Endpoints.MatchProjects,
                request,
                ct);

            response.EnsureSuccessStatusCode();

            // 3. Парсинг ответа
            var result = await response.Content.ReadFromJsonAsync<List<ProjectMatchResponse>>(cancellationToken: ct);

            return result;
        }
    }
}
