using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Persistence.Contexts;

namespace ReleaseNotes_WebAPI.Persistence.Repositories
{
    public class MappableRepository : BaseRepository, IMappableRepository
    {
        
        
        public MappableRepository(AppDbContext context) : base(context)
        {
        }
        
        public async Task<IEnumerable<MappableField>> GetMappableFields()
        {
           return await _context.MappableFields.ToListAsync();
        }

        public async Task<IEnumerable<ReleaseNoteMapping>> GetMappedFields()
        {
            return await _context.ReleaseNoteMappings.Include(
                rrm => rrm.MappableField).ToListAsync();
        }
    }
}