using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Persistence.Contexts;
using ReleaseNotes_WebAPI.Utilities;

namespace ReleaseNotes_WebAPI.Persistence.Repositories
{
    public class ReleaseRepository : BaseRepository, IReleaseRepository
    {
        public ReleaseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Release>> ListAsync(ReleasesParameters queryParameters)
        {
            {
                IQueryable<Release> releasesQuery = _context.Releases.AsNoTracking();

                if (queryParameters.product > 0)
                {
                    releasesQuery = releasesQuery.Where(r => r.ProductVersion.Product.Id == queryParameters.product);
                }

                if (!String.IsNullOrEmpty(queryParameters.sort))
                {
                    switch (queryParameters.sort)
                    {
                        case "old":
                            releasesQuery = releasesQuery.OrderBy(r => r.Date);
                            break;
                        case "new":
                            releasesQuery = releasesQuery.OrderByDescending(r => r.Date);
                            break;
                    }
                }

                return await releasesQuery
                    .Include(r => r.ReleaseReleaseNotes)
                    .ThenInclude(rrn => rrn.ReleaseNote)
                    .Include(r => r.ProductVersion).ThenInclude(pv => pv.Product)
                    .ToListAsync();
            }
        }

        public async Task AddAsync(Release release)
        {
            await _context.Releases.AddAsync(release);
        }

        public void Remove(Release release)
        {
            _context.Releases.Remove(release);
        }

        public void Update(Release release)
        {
            _context.Releases.Update(release);
        }

        public async Task<Release> FindByIdAsync(int id)
        {
            return await _context.Releases
                .Include(r => r.ProductVersion)
                .ThenInclude(pv => pv.Product)
                .Include(r => r.ReleaseReleaseNotes)
                .ThenInclude(rrn => rrn.ReleaseNote)
                .SingleOrDefaultAsync(r => r.Id == id);
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