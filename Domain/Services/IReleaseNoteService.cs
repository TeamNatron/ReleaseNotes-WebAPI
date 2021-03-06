﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Services.Communication;
using ReleaseNotes_WebAPI.Resources;
using ReleaseNotes_WebAPI.Utilities;

namespace ReleaseNotes_WebAPI.Domain.Services
{
    public interface IReleaseNoteService
    {
        Task<IEnumerable<ReleaseNote>> ListAsync();

        Task<IEnumerable<ReleaseNote>> FilterDates(ReleaseNoteParameters queryParameters);

        Task<ReleaseNoteResponse> GetReleaseNote(int id, bool includeReleases);

        Task<ReleaseNoteResponse> RemoveReleaseNote(int id);

        Task<ReleaseNoteResponse> UpdateReleaseNote(int id, EditReleaseNoteResource note);
        
        Task<ReleaseNoteResponse> CreateReleaseNote(CreateReleaseNoteResource note);
        Task<ReleaseNotesResponse> CreateReleaseNotesFromMap(JArray mapFrom);
    }
}