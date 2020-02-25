using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Domain.Repositories
{
    public interface IProductVersionRepository
    {
        Task<IEnumerable<ProductVersion>> ListAsync();
    }
}