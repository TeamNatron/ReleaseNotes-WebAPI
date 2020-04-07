using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Utilities;

namespace ReleaseNotes_WebAPI.Domain.Repositories
{
    public interface IReleaseNoteRepository
    {
        Task<IEnumerable<ReleaseNote>> ListAsync();

        Task<IEnumerable<ReleaseNote>> FilterDates(ReleaseNoteParameters queryParameters);
        
        void AddAsync(ReleaseNote releaseNote);

        Task<ReleaseNote> FindAsync(int id, bool includeReleases);
        Task<ReleaseNote> FindAsync(int id);

        void UpdateReleaseNote(ReleaseNote note);

        void Remove(ReleaseNote releaseNote);
    }
}