using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Domain.Services.Communication;

namespace ReleaseNotes_WebAPI.Services
{
    public class ImageService : IImageService
    {
        private readonly string _imagesDirectory = "images";
        private readonly string _storedImagesPath;
        private readonly string _baseUrl;

        public ImageService(IWebHostEnvironment environment, IHttpContextAccessor httpContext)
        {
            _baseUrl = Path.Combine("http://" + httpContext.HttpContext.Request.Host.Value, _imagesDirectory);
            _storedImagesPath = Path.Combine(environment.WebRootPath, _imagesDirectory);
        }

        public async Task<ImageResponse> SaveToFilesystem(IFormFile file)
        {
            if (file.Length <= 0) return new ImageResponse(false, "Image cannot be empty");

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_storedImagesPath, fileName);
            await using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = Path.Combine(_baseUrl, fileName);

            return new ImageResponse(imageUrl);
        }
    }
}