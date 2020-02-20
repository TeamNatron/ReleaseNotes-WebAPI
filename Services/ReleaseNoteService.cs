using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Domain.Services.Communication;
using ReleaseNotes_WebAPI.Resources;

namespace ReleaseNotes_WebAPI.Services
{
    public class ReleaseNoteService : IReleaseNoteService
    {
        private readonly IReleaseNoteRepository _releaseNoteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReleaseNoteService(IReleaseNoteRepository releaseNoteRepository, IUnitOfWork unitOfWork)
        {
            _releaseNoteRepository = releaseNoteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ReleaseNote>> ListAsync()
        {
            return await _releaseNoteRepository.ListAsync();
        }

        public async Task<ReleaseNote> GetReleaseNote(int id)
        {
            var existingReleaseNote = await _releaseNoteRepository.FindAsync(id);
            if (existingReleaseNote == null)
            {
                // TODO: improve error handling
                return existingReleaseNote;
            }

            try
            {
                // the note exists, return it

                return existingReleaseNote;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Det oppsto en feil: {e.Message}");
                return existingReleaseNote;
            }
        }

        public async Task<ReleaseNoteResponse> UpdateReleaseNote(EditReleaseNoteResource note)
        {
            var existingReleaseNote = await _releaseNoteRepository.FindAsync(note.Id);
            if (existingReleaseNote == null)
            {
                return new ReleaseNoteResponse("Release noten finnes ikke!");
            }

            try
            {
                existingReleaseNote.Title = note.Title;
                existingReleaseNote.Ingress = note.Ingress;
                existingReleaseNote.DetailedView = note.DetailedView;
                existingReleaseNote.AuthorEmail = note.AuthorEmail;
                existingReleaseNote.AuthorName = note.AuthorName;
                existingReleaseNote.IsPublic = note.IsPublic;
                existingReleaseNote.WorkItemTitle = note.WorkItemTitle;
                _releaseNoteRepository.Update(existingReleaseNote);
                await _unitOfWork.CompleteAsync();
                return new ReleaseNoteResponse(existingReleaseNote);
            }
            catch (Exception e)
            {
                return new ReleaseNoteResponse($"Det oppsto en feil: {e.Message}");
            }
        }
    }
}