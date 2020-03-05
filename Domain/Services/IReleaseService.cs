using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Services.Communication;
using ReleaseNotes_WebAPI.Resources;
using ReleaseNotes_WebAPI.Utilities;

namespace ReleaseNotes_WebAPI.Domain.Services
{
    public interface IReleaseService
    {
        Task<ReleaseResponse> SaveAsync(SaveReleaseResource release);
        Task<ReleaseResponse> UpdateAsync(int id, SaveReleaseResource release);
        Task<ReleaseResponse> RemoveRelease(int id);
        Task<ReleaseResponse> GetRelease(int id);
        Task<IEnumerable<Release>> ListAsync(ReleasesParameters queryParameters);
    }
}