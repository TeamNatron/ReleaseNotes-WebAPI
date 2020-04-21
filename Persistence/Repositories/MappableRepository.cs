using System;
using System.Collections.Generic;
using System.Linq;
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
            var query = _context.MappableFields.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ReleaseNoteMapping>> GetMappedFields(string type)
        {
            var query = _context.ReleaseNoteMappings.AsTracking();
            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(m => m.MappableType.Name.ToUpper() == type.ToUpper());
            }

            return await query
                .Include(rnm => rnm.MappableField)
                .Include(rnm => rnm.MappableType)
                .ToListAsync();
        }

        public async Task<ReleaseNoteMapping> FindAsync(string type, string mappableField)
        {
            return await _context.ReleaseNoteMappings
                .Include(rnm => rnm.MappableField)
                .Include(rnm => rnm.MappableType)
                .SingleOrDefaultAsync(rnm => 
                    rnm.MappableField.Name.ToUpper() == mappableField.ToUpper() &&
                    rnm.MappableType.Name.ToUpper() == type.ToUpper());
        }

        public void UpdateReleaseNoteMappingAsync(ReleaseNoteMapping existingMapping)
        {
            _context.ReleaseNoteMappings.Update(existingMapping);
        }
    }
}