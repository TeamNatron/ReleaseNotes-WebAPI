using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Domain.Services.Communication;

namespace ReleaseNotes_WebAPI.Services
{
    public class ImageService : IImageService
    {
        private readonly string _imagesDirectory = "images";
        private const string Api = "api";
        private readonly string _storedImagesPath;
        private readonly string _baseUrl;
        private readonly IImageRepository _imageRepository;

        public ImageService(IWebHostEnvironment environment, IHttpContextAccessor httpContext, IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
            _baseUrl = Path.Combine("http://" + httpContext.HttpContext.Request.Host.Value, Api, _imagesDirectory);
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

        public async Task<FileStream> GetAsync(string imageUrl)
        {
            return await _imageRepository.GetAsync(imageUrl);
        }
    }
}