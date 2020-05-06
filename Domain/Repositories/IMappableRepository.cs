using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Resources;

namespace ReleaseNotes_WebAPI.Domain.Repositories
{
    public interface IMappableRepository
    {
        Task<IEnumerable<MappableField>> GetMappableFields();
        Task<IEnumerable<ReleaseNoteMapping>> GetMappedFields(string type);
        Task<ReleaseNoteMapping> FindAsync(string type, string mappableField);

        void UpdateReleaseNoteMappingAsync(ReleaseNoteMapping existingMapping);
    }
}