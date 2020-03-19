using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Domain.Services.Communication;
using ReleaseNotes_WebAPI.Resources;
using ReleaseNotes_WebAPI.Utilities;

namespace ReleaseNotes_WebAPI.Services
{
    public class ReleaseNoteService : IReleaseNoteService
    {
        private readonly IReleaseNoteRepository _releaseNoteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReleaseNoteService(IReleaseNoteRepository releaseNoteRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _releaseNoteRepository = releaseNoteRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReleaseNote>> ListAsync()
        {
            return await _releaseNoteRepository.ListAsync();
        }

        public async Task<IEnumerable<ReleaseNote>> FilterDates(ReleaseNoteParameters queryParameters)
        {
            return await _releaseNoteRepository.FilterDates(queryParameters);
        }

        public async Task<ReleaseNoteResponse> GetReleaseNote(int id)
        {
            var existingReleaseNote = await _releaseNoteRepository.FindAsync(id);
            if (existingReleaseNote == null)
            {
                return new ReleaseNoteResponse("Release noten eksisterer ikke!");
            }

            try
            {
                return new ReleaseNoteResponse(existingReleaseNote);
            }
            catch (Exception e)
            {
                return new ReleaseNoteResponse($"Det oppsto en feil: {e.Message}");
            }
        }


        public async Task<ReleaseNoteResponse> RemoveReleaseNote(int id)
        {
            var existingReleaseNote = await _releaseNoteRepository.FindAsync(id);
            if (existingReleaseNote == null)
            {
                return new ReleaseNoteResponse("Release noten eksisterer ikke!");
            }

            try
            {
                _releaseNoteRepository.Remove(existingReleaseNote);
                await _unitOfWork.CompleteAsync();
                return new ReleaseNoteResponse(existingReleaseNote);
            }
            catch (Exception e)
            {
                return new ReleaseNoteResponse($"Det oppsto en feil: {e.Message}");
            }
        }

        public async Task<ReleaseNoteResponse> UpdateReleaseNote(int id, EditReleaseNoteResource note)
        {
            var existingReleaseNote = await _releaseNoteRepository.FindAsync(id);
            if (existingReleaseNote == null)
            {
                return new ReleaseNoteResponse("Release noten finnes ikke!");
            }

            try
            {
                existingReleaseNote.Title = note.Title;
                existingReleaseNote.Ingress = note.Ingress;
                existingReleaseNote.Description = note.Description;
                existingReleaseNote.IsPublic = note.IsPublic;
                _releaseNoteRepository.UpdateReleaseNote(existingReleaseNote);
                await _unitOfWork.CompleteAsync();
                return new ReleaseNoteResponse(existingReleaseNote);
            }
            catch (Exception e)
            {
                return new ReleaseNoteResponse($"Det oppsto en feil: {e.Message}");
            }
        }

        public async Task<ReleaseNoteResponse> CreateReleaseNote(EditReleaseNoteResource note)
        {
            try
            {
                var noteModel = _mapper.Map<ReleaseNote>(note);
                noteModel.ClosedDate = DateTime.Now;
                _releaseNoteRepository.AddAsync(noteModel);
                await _unitOfWork.CompleteAsync();
                return new ReleaseNoteResponse(noteModel);
            }
            catch (Exception e)
            {
                return new ReleaseNoteResponse($"Det oppsto en feil: {e.Message}");
            }
        }
    }
}