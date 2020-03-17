using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Persistence.Contexts;
using ReleaseNotes_WebAPI.Utilities;

namespace ReleaseNotes_WebAPI.Persistence.Repositories
{
    public class ReleaseNoteRepository : BaseRepository, IReleaseNoteRepository
    {
        public ReleaseNoteRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ReleaseNote>> ListAsync(ReleaseNoteParameters queryParameters)
        {
            
            // TODO: flytt koden inn i service
            // get a immutable copy of the release notes
            IQueryable<ReleaseNote> releaseNoteQuery = _context.ReleaseNotes.AsNoTracking();
            
            //asdasd
            releaseNoteQuery.filterDateQuery();

            // see if date filtering is requested:
            if (queryParameters.StartDate.HasValue && queryParameters.EndDate.HasValue)
            {
                // re-assign to proper dateTime objects, instead of nullable DateTime
                DateTime start = queryParameters.StartDate.Value;
                DateTime end = queryParameters.EndDate.Value;
                // determine which dates comes first, StartDate or EndDate
                int result = start.CompareTo(end);
                if (result < 0)
                {
                    releaseNoteQuery = releaseNoteQuery.Where(rn => rn.ClosedDate > start && rn.ClosedDate <= end);
                }

                if (result == 0)
                {
                    // startDate and endDate are the same, odd but okay
                    releaseNoteQuery = releaseNoteQuery.Where(rn =>
                        rn.ClosedDate.CompareTo(start) == 0);
                }

                if (result > 0)
                {
                    // startDate is after endDate
                    // inform the user of the blunder
                    // send the user some errors
                    Console.WriteLine("The end date is before the start date");
                    BadFilterRequest
                    
                }

                return await releaseNoteQuery.ToListAsync();
            }

            // no date filtering is requested:
            // querypararm way
            return await releaseNoteQuery.ToListAsync();

            // old non-queryParam way
            //return await _context.ReleaseNotes.ToListAsync(queryParameters);
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

        public void Remove(ReleaseNote releaseNote)
        {
            _context.ReleaseNotes.Remove(releaseNote);
        }
    }
}