﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Domain.Services.Communication;
using ReleaseNotes_WebAPI.Persistence.Contexts;
using ReleaseNotes_WebAPI.Utilities;

namespace ReleaseNotes_WebAPI.Persistence.Repositories
{
    public class ReleaseNoteRepository : BaseRepository, IReleaseNoteRepository
    {
        public ReleaseNoteRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ReleaseNote>> ListAsync()
        {
            IQueryable<ReleaseNote> releaseNoteQuery = _context.ReleaseNotes.AsNoTracking();
            return await releaseNoteQuery.ToListAsync();
        }

        public async void AddAsync(ReleaseNote releaseNote)
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