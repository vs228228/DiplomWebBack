using Microsoft.AspNetCore.Http;

namespace DiplomWebBack.Domain.Interfaces
{
    public interface IFileService
    {
        Task<List<string>> SaveFilesAsync(List<IFormFile> files, List<string> names);
    }
}
