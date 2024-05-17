using Microsoft.AspNetCore.Http;

namespace RookieEShopper.Infrastructure.Services
{
    public class FileService
    {
        public async Task uploadImage(string path, IFormFile file)
        {
            using (var stream = File.Create(path))
            {
                await file.CopyToAsync(stream);
            }
        }
    }
}