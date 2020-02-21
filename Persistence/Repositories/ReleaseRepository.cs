using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Persistence.Contexts;

namespace ReleaseNotes_WebAPI.Persistence.Repositories
{
    public class ReleaseRepository : BaseRepository, IReleaseRepository
    {
        public ReleaseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Release release)
        {
            await _context.Releases.AddAsync(release);
        }
        
        public async Task<Release> FindByNameAsync(string releaseName)
        {
            return await _context.Releases.Where(r => r.Title == releaseName).SingleOrDefaultAsync();
        }

        public async Task<ProductVersion> FindProductVersion(int id)
        {
            return await _context.ProductVersions.FindAsync(id);
        }

        public async Task<ICollection<ReleaseNote>> FindReleaseNotes(IEnumerable<int> ids)
        {
            return await _context.ReleaseNotes.Where(rn => ids.Contains(rn.Id)).ToListAsync();
        }
    }
}