using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Services.Communication;

namespace ReleaseNotes_WebAPI.Domain.Services
{
    public interface IProductVersionService
    {
        Task<IEnumerable<ProductVersion>> ListAsync();
        Task<ProductVersionResponse> AddAsync(ProductVersion productVersion);

    }
}