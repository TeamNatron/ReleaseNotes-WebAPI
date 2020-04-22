using System.IO;
using System.Threading.Tasks;

namespace ReleaseNotes_WebAPI.Domain.Repositories
{
    public interface IImageRepository
    {
        public Task<FileStream> GetAsync(string imageUrl);
    }
}