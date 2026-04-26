using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Entities.Responses;
using DiplomWebBack.Domain.Interfaces;
using DiplomWebBack.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        // Вспомогательный тип для десериализации ответа внешнего API,
        // где project_id приходит как int (индекс), а не как GUID.
        private class ExternalProjectMatchResponse
        {
            [JsonPropertyName("project_id")]
            public int ProjectId { get; set; }

            [JsonPropertyName("project_name")]
            public string ProjectName { get; set; }

            [JsonPropertyName("score")]
            public double Score { get; set; }

            [JsonPropertyName("reason")]
            public List<string> Reason { get; set; }
        }

        public async Task<List<ProjectMatchResponse>> MatchProjectsAsync(
            UserSkillsDocument userSkills,
            IEnumerable<Project> projects,
            CancellationToken ct)
        {
            // ⚠️ убираем FirstOrDefault-костыль (он ломает данные)
            var projectList = projects?
                .Where(p => p != null)
                .ToList() ?? new List<Project>();

            // 1. Кандидат (нормализация null → 0)
            var candidateSkills = userSkills.Skills
                .Where(s => !string.IsNullOrWhiteSpace(s.Name))
                .Select(s => new
                {
                    name = s.Name.Trim(),
                    years = s.Years ?? 0
                })
                .ToList();

            // 2. Проекты
            // Внешнее API ожидает integer id (индекс) вместо GUID.
            // Создаём DTO с целочисленным id (позиция в списке) и сохраняем
            // соответствие индекса -> Project для обратного преобразования.
            var indexedProjects = projectList
                .Select((p, idx) => new { Index = idx, Project = p })
                .ToList();

            var projectDtos = indexedProjects.Select(ip => new
            {
                id = ip.Index, // отправляем int
                name = ip.Project.Title,
                skills = ip.Project.ProjectTags
                    .Where(pt => pt.Tag != null && !string.IsNullOrWhiteSpace(pt.Tag.Title))
                    .Select(pt => new
                    {
                        name = pt.Tag.Title.Trim(),

                        // 🔥 главное исправление — null → 0
                        required_years = (double?)pt.Year ?? 0.0,

                        weight = (double?)pt.Weight ?? 0.0
                    })
                    .ToList()
            })
            .ToList();

            var request = new
            {
                candidate = new
                {
                    skills = candidateSkills
                },
                projects = projectDtos
            };

            // 🔍 debug (оставь пока полезно)
            var json = System.Text.Json.JsonSerializer.Serialize(request, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });

            Console.WriteLine(json);

            // 3. Запрос
            var response = await _httpClient.PostAsJsonAsync(
                _options.Endpoints.MatchProjects,
                request,
                ct);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(ct);
                Console.WriteLine($"External API error: {error}");
            }

            response.EnsureSuccessStatusCode();

            // 4. Ответ
            // Сначала десериализуем во временный тип, где project_id — int.
            var externalResult = await response.Content
                .ReadFromJsonAsync<List<ExternalProjectMatchResponse>>(cancellationToken: ct);

            // Преобразуем в доменный тип, восстанавливая исходный GUID проекта по индексу.
            var mapped = new List<ProjectMatchResponse>();

            if (externalResult != null)
            {
                foreach (var ext in externalResult)
                {
                    // Проверяем корректность индекса, чтобы не упасть при некорректных данных от внешнего API.
                    Guid projectGuid = Guid.Empty;
                    if (ext.ProjectId >= 0 && ext.ProjectId < indexedProjects.Count)
                    {
                        projectGuid = indexedProjects[ext.ProjectId].Project.Id;
                    }
                    else
                    {
                        // Если индекс некорректен — логируем и пропускаем / ставим Guid.Empty.
                        Console.WriteLine($"External API returned unknown project_id: {ext.ProjectId}");
                    }

                    mapped.Add(new ProjectMatchResponse
                    {
                        ProjectId = projectGuid,
                        ProjectName = ext.ProjectName,
                        Score = ext.Score,
                        Reason = ext.Reason ?? new List<string>()
                    });
                }
            }

            return mapped;
        }
    }
}
