using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Services.Communication;
using ReleaseNotes_WebAPI.Resources;

namespace ReleaseNotes_WebAPI.Domain.Services
{
    public interface IMappableService
    {
        Task<MappedResponse> ListMappedAsync(string type);

        Task<ReleaseNoteMapping> GetMappedByCompKey(string type, string mappableField);
        Task<MappableResponse> ListMappableAsync();
        Task<MappingResponse> UpdateReleaseNoteMappingAsync(UpdateReleaseNoteMappingResource resource, string type,
            string mappableField);
    }
}