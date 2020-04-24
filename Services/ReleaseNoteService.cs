using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public async Task<ReleaseNotesResponse> CreateReleaseNotesFromMap(JObject mapFrom, string mappableType)
        {
            var mappings = _mappableService.ListMappedAsync(mappableType);
            var workItems = mapFrom["value"];
            var notesToSave = new List<ReleaseNote>();
            if (workItems.GetType() == typeof(JArray))
            {
                foreach (var workItem in workItems.Children())
                {
                    var note = new ReleaseNote();
                    var allTokens = AllTokens(workItem);
                    foreach (var mapping in mappings.Result.Entity)
                    {
                        if (mapping.AzureDevOpsField != null)
                        {
                            var value = GetValueFromField(allTokens, mapping.AzureDevOpsField);
                            
                            /*var mapPath = mapping.AzureDevOpsField.Split("/");
                            var value = "";
                            JToken tmp = null;
                            foreach (var field in mapPath)
                            {
                                Console.WriteLine(field);
                                tmp = workItem[field];
                            }
                            if (tmp != null)
                            {
                                value = tmp.Value<string>();
                            }*/
                            
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                SetField(note, mapping.MappableField, value);
                            }
                        }
                    }
                    notesToSave.Add(note);
                }
            }
            _releaseNoteRepository.AddRangeAsync(notesToSave);

            await _unitOfWork.CompleteAsync();
            return new ReleaseNotesResponse(true, mapFrom.ToString(), notesToSave);
        }

        /**
         * Set value of a field in a ReleaseNote using the string name of the field
         */
        private void SetField(ReleaseNote item, string dst, string value)
        {
            var prop = item.GetType().GetProperty(dst);
            if (prop != null)
            {
                prop.SetValue(item, value);
            }
        }

        /*
         * Get value from a field using its string name
         */
        private string GetValueFromField(IEnumerable<JToken> allTokens, string fieldToMapTo)
        {
            var value = "";
            if (!string.IsNullOrWhiteSpace(fieldToMapTo))
            {
                var field = allTokens.FirstOr(
                    t => t.Type == JTokenType.Property && ((JProperty) t).Name == fieldToMapTo, "");
                value = field.Value<string>();
                return value;
            }
        }

        /**
         * Extract all nested  fields of an object to an Enumarable list 
         */
        private IEnumerable<JToken> AllTokens(JToken obj)
        {
            var toSearch = new Stack<JToken>(obj.Children());
            while (toSearch.Count > 0)
            {
                var inspected = toSearch.Pop();
                yield return inspected;
                foreach (var child in inspected)
                {
                    toSearch.Push(child);
                }
            }
        }
    }
}