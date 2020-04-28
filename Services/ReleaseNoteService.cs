using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
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

        public async Task<ReleaseNotesResponse> CreateReleaseNotesFromMap(JArray workItems)
        {
            var notes = await DoCreateReleaseNotesFromMap(workItems);
            _releaseNoteRepository.AddRangeAsync(notes);
            await _unitOfWork.CompleteAsync();
            return new ReleaseNotesResponse(true, "0px", notes);
        }

        protected async Task<List<ReleaseNote>> DoCreateReleaseNotesFromMap(JArray workItems)
        {
            var mappableResponse = await _mappableService.ListMappableAsync();
            var mappableFieldsResources = mappableResponse.Entity.ToList();

            var notesToSave = new List<ReleaseNote>();

            foreach (var workItem in workItems.Children())
            {
                var fields = workItem["fields"];
                var type = fields["System.WorkItemType"].Value<string>();
                var note = new ReleaseNote();

                // perform mapping
                foreach (var mappable in mappableFieldsResources)
                {
                    var mapping = await _mappableService.GetMappedByCompKey(type, mappable.Name);
                    var devOpsField = mapping.AzureDevOpsField;

                    if (string.IsNullOrWhiteSpace(devOpsField)) continue;

                    var value = GetValueFromField(fields, devOpsField, mappable.DataType);
                    
                    if (value != null)
                    {
                        SetField(note, mapping.MappableField.Name, value);
                    }
                }

                // assign non-mappable fields
                note.AuthorEmail = fields["System.AssignedTo"]["uniqueName"].Value<string>();
                note.AuthorName = fields["System.AssignedTo"]["displayName"].Value<string>();
                note.WorkItemId = workItem["id"].Value<int>();

                // save
                notesToSave.Add(note);
            }

            return notesToSave;
        }

        /**
         * Set value of a field in a ReleaseNote using the string name of the field
         */
        private void SetField(ReleaseNote item, string dst, object value)
        {
            var prop = item.GetType().GetProperty(dst);
            if (prop != null)
            {
                prop.SetValue(item, value);
            }
        }

        /*
         * Get value from a field using its string name and specified MappableDataType
         */
        private object GetValueFromField(JToken fields, string fieldName, string dataType)
        {
            dynamic value = null;
            if (string.IsNullOrWhiteSpace(fieldName)) return value;

            var field = fields[fieldName];
            
            if (field == null) return value;

            switch (dataType)
            {
                case MappableDataTypes.String:
                case MappableDataTypes.Html:
                    value = field.Value<string>();;
                    break;
                case MappableDataTypes.DateTime:
                    value = field.Value<DateTime>();
                    break;
                default:
                    Console.WriteLine("ERROR: no match for dataType " + dataType);
                    break;
            }

            Console.WriteLine(value);
            return value;
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