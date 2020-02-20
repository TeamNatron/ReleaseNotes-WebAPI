using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Persistence.Contexts;

namespace ReleaseNotes_WebAPI.Persistence.Repositories
{
    public class ReleaseNoteRepository : BaseRepository, IReleaseNoteRepository
    {
        public ReleaseNoteRepository(AppDbContext context) : base(context)
        {
        }

        // basic list all
        public async Task<IEnumerable<ReleaseNote>> ListAsync()
        {
            return await _context.ReleaseNotes.ToListAsync();
        }

        public async Task AddAsync(ReleaseNote releaseNote)
        {
            await _context.ReleaseNotes.AddAsync(releaseNote);
        }

        public async Task<ReleaseNote> FindAsync(int id)
        {
            return await _context.ReleaseNotes.FindAsync(id);
        }

        public void UpdateReleaseNote(ReleaseNote note)
        {
            _context.ReleaseNotes.Update(note);
        }

        public void Update(ReleaseNote releaseNote)
        {
            _context.ReleaseNotes.Update(releaseNote);
        }

        public void Remove(ReleaseNote releaseNote)
        {
            _context.ReleaseNotes.Remove(releaseNote);
        }
    }
}