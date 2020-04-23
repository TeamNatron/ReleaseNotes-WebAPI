﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json.Linq;
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
        private readonly IMappableService _mappableService;

        public ReleaseNoteService(IReleaseNoteRepository releaseNoteRepository, IMappableService mappableService,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _releaseNoteRepository = releaseNoteRepository;
            _mappableService = mappableService;
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

        public async Task<ReleaseNoteResponse> GetReleaseNote(int id, bool includeReleases)
        {
            ReleaseNote existingReleaseNote;
            if (includeReleases)
            {
                existingReleaseNote = await _releaseNoteRepository.FindAsync(id, true);
            }
            else
            {
                existingReleaseNote = await _releaseNoteRepository.FindAsync(id);
            }

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
                _mapper.Map(note, existingReleaseNote);
                _releaseNoteRepository.UpdateReleaseNote(existingReleaseNote);
                await _unitOfWork.CompleteAsync();
                return new ReleaseNoteResponse(existingReleaseNote);
            }
            catch (Exception e)
            {
                return new ReleaseNoteResponse($"Det oppsto en feil: {e.Message}");
            }
        }

        public async Task<ReleaseNoteResponse> CreateReleaseNote(CreateReleaseNoteResource note)
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

        public async Task<ReleaseNoteResponse> CreateReleaseNoteFromMap(JObject mapFrom, string mappableType)
        {
            var mappings = _mappableService.ListMappedAsync(mappableType);
            var note = new ReleaseNote();

            foreach (var mapping in mappings.Result.Entity)
            {
                var value = GetValueFromField(mapFrom, mapping.AzureDevOpsField);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    SetField(note, mapping.MappableField, value);
                }
            }

            _releaseNoteRepository.AddAsync(note);
            await _unitOfWork.CompleteAsync();
            return new ReleaseNoteResponse(true, mapFrom.ToString(), note);
        }

        private void SetField(ReleaseNote item, string dst, string value)
        {
            var prop = item.GetType().GetProperty(dst);
            if (prop != null)
            {
                prop.SetValue(item, value);
            }
        }

        private string GetValueFromField(JObject mapFrom, string field)
        {
            var value = "";
            if (!string.IsNullOrWhiteSpace(field))
            {
                var fieldToMapTo = field;
                var p1 = mapFrom["fields"];
                if (p1 == null)
                {
                    // "Fields" does not exist.
                    throw new Exception("F");
                }

                var p2 = p1[fieldToMapTo];
                if (p2 != null)
                {
                    value = p2.Value<string>();
                }
                else
                {
                    // target AzureDevopsField does not exist
                }
            }

            return value;
        }
    }
}