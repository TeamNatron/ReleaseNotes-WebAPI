using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Services.Communication;
using ReleaseNotes_WebAPI.Resources;

namespace ReleaseNotes_WebAPI.Domain.Services
{
    public interface IReleaseNoteService
    {
        Task<IEnumerable<ReleaseNote>> ListAsync();

        Task<ReleaseNote> GetReleaseNote(int id);

        Task<ReleaseNoteResponse> UpdateReleaseNote(EditReleaseNoteResource note);
    }
}