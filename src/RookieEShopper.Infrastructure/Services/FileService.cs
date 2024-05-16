using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
