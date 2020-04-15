using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Resources;

namespace ReleaseNotes_WebAPI.Domain.Services
{
    public interface IMappableService
    {
        Task<IEnumerable<MappableField>> ListAsync();
    }
}