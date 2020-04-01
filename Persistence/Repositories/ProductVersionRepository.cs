using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Persistence.Contexts;

namespace ReleaseNotes_WebAPI.Persistence.Repositories
{
    public class ProductVersionRepository : BaseRepository, IProductVersionRepository
    {
        public ProductVersionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProductVersion>> ListAsync()
        {
            return await _context.ProductVersions.Include(pv => pv.Product).ToListAsync();
        }

        public async Task AddAsync(ProductVersion productVersion)
        {
            await _context.ProductVersions.AddAsync(productVersion);
        }

        public async Task<bool> AnyAsync(ProductVersion version)
        {
            return await _context.ProductVersions.AnyAsync(pv =>
                pv.ProductId == version.ProductId && pv.Version == version.Version);
        }
    }
}