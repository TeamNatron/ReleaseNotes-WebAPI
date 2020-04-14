using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ReleaseNotes_WebAPI.Domain.Services.Communication;

namespace ReleaseNotes_WebAPI.Domain.Services
{
    public interface IImageService
    {
        Task<ImageResponse> SaveToFilesystem(IFormFile file);

    }
}