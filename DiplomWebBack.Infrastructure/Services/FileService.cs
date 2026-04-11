using DiplomWebBack.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DiplomWebBack.Infrastructure.Services
{
    public class FileService : IFileService
    {
        public async Task<List<string>> SaveFilesAsync(List<IFormFile> files, List<string> names)
        {
            var resultPaths = new List<string>();

            if (files == null || files.Count == 0)
            {
                return resultPaths;
            }

            var rootPath = Directory.GetCurrentDirectory();

            var lastDotIndex = rootPath.LastIndexOf('.');

            if (lastDotIndex != -1)
            {
                rootPath = rootPath.Substring(0, lastDotIndex) + ".Api";
            }

            var uploadFolder = Path.Combine(rootPath, "wwwroot", "uploads");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            int i = 0;

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    continue;
                }

                var fileName = names[i];
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                resultPaths.Add($"/uploads/{fileName}");
                i++;
            }

            return resultPaths;
        }
    }
}
