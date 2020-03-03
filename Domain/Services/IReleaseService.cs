using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Services.Communication;
using ReleaseNotes_WebAPI.Resources;

namespace ReleaseNotes_WebAPI.Domain.Services
{
    public interface IReleaseService
    {
        Task<ReleaseResponse> SaveAsync(SaveReleaseResource release);
        Task<ReleaseResponse> UpdateAsync(int id, SaveReleaseResource release);
        Task<ReleaseResponse> GetRelease(int id);
    }
}