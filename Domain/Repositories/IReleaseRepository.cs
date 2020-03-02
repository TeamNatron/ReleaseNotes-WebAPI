using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Resources;
using ReleaseNotes_WebAPI.Utilities;

namespace ReleaseNotes_WebAPI.Domain.Repositories
{
    public interface IReleaseRepository
    {
        Task<IEnumerable<Release>> ListAsync(ReleasesParameters queryParameters);
        Task AddAsync(Release release);

        void Update(Release release);
        Task<Release> FindByIdAsync(int id);

        Task<Release> FindByNameAsync(string release);

        Task<ProductVersion> FindProductVersion(int id);

        Task<ICollection<ReleaseNote>> FindReleaseNotes(IEnumerable<int> ids);
    }
}