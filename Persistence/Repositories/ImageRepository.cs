using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReleaseNotes_WebAPI.Domain.Repositories;

namespace ReleaseNotes_WebAPI.Persistence.Repositories
{
    public class ImageRepository : IImageRepository
    {

        private const string BaseUrl = "/app/wwwroot/images/";

        public async Task<FileStream> GetAsync(string imageUrl)
        {
            return await Task.Run(() => File.OpenRead(BaseUrl + imageUrl));
        }
    }
}