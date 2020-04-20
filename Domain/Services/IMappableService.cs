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
        Task<MappingResponse> ListAsync(bool mapped);
        
        Task<MappingResponse> UpdateReleaseNoteMappingAsync(UpdateReleaseNoteMappingResource resource, int id);
    }
}