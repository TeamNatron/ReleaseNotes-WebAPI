using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Resources;

namespace ReleaseNotes_WebAPI.Domain.Repositories
{
    public interface IMappableRepository
    {
        Task<IEnumerable<MappableField>> GetMappableFields();
        Task<IEnumerable<ReleaseNoteMapping>> GetMappedFields();
        Task<ReleaseNoteMapping> FindAsync(int id);
        void UpdateReleaseNoteMappingAsync(ReleaseNoteMapping existingMapping);
    }
}