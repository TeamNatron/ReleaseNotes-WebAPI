using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Domain.Repositories
{
    public interface IReleaseNoteRepository
    {
        Task<IEnumerable<ReleaseNote>> ListAsync();
        
        void AddAsync(ReleaseNote releaseNote);

        Task<ReleaseNote> FindAsync(int id);

        void UpdateReleaseNote(ReleaseNote note);

        void Remove(ReleaseNote releaseNote);
    }
}