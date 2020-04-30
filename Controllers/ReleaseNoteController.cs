using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Extensions;
using ReleaseNotes_WebAPI.Resources;
using ReleaseNotes_WebAPI.Utilities;

namespace ReleaseNotes_WebAPI.Controllers
{
    [Route("/api/[controller]")]
    public class ReleaseNoteController : Controller
    {
        private readonly IReleaseNoteService _releaseNoteService;
        private readonly IMapper _mapper;

        public ReleaseNoteController(IReleaseNoteService releaseNoteService, IMapper mapper)
        {
            _releaseNoteService = releaseNoteService;
            _mapper = mapper;
        }

        // GET: api/releasenote
        [HttpGet]
        public async Task<ActionResult<ReleaseNoteResource>> GetAllAsync(
            [FromQuery] ReleaseNoteParameters queryParameters)
        {
            // see if date filtering is required
            if (queryParameters.StartDate.HasValue && queryParameters.EndDate.HasValue)
            {
                // Check if the endDate happens before the startDate
                if (queryParameters.StartDate.Value.CompareTo(queryParameters.EndDate.Value) > 0)
                {
                    return BadRequest(ModelState);
                }

                var filteredDates = await _releaseNoteService.FilterDates(queryParameters);
                var res = _mapper.Map<IEnumerable<ReleaseNoteResource>>(filteredDates);
                return Ok(res);
            }

            // no date filtering needed, return all release notes
            var releaseNotes = await _releaseNoteService.ListAsync();
            var resources = _mapper.Map<IEnumerable<ReleaseNoteResource>>(releaseNotes);
            return Ok(resources);
        }

        // GET: api/releasenote/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ReleaseNoteResource>> GetSpecificReleaseNote(
            [FromQuery] bool includeReleases,
            int id)
        {
            var releaseNoteResponse = await _releaseNoteService.GetReleaseNote(id, includeReleases);
            if (releaseNoteResponse == null)
            {
                return NotFound();
            }

            var releaseNote = _mapper.Map<ReleaseNote, ReleaseNoteResource>(releaseNoteResponse.ReleaseNote);
            return Ok(releaseNote);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = ("Administrator"))]
        public async Task<ActionResult<ReleaseNoteResource>> UpdateReleaseNoteAsync(int id,
            [FromBody] EditReleaseNoteResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var result = await _releaseNoteService.UpdateReleaseNote(id, resource);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var releaseNoteResource = _mapper.Map<ReleaseNote, ReleaseNoteResource>(result.ReleaseNote);
            return Ok(releaseNoteResource);
        }

        [HttpPost()]
        [Authorize(Roles = ("Administrator"))]
        public async Task<ActionResult<ReleaseNoteResource>> CreateReleaseNoteAsync(
            [FromBody] CreateReleaseNoteResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var result = await _releaseNoteService.CreateReleaseNote(resource);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var releaseNoteResource = _mapper.Map<ReleaseNote, ReleaseNoteResource>(result.ReleaseNote);
            return Ok(releaseNoteResource);
        }

        [HttpPost("azure")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ReleaseNoteResource>> CreateReleaseNoteFromMapAsync([FromBody] JArray resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var result = await _releaseNoteService.CreateReleaseNotesFromMap(resource);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var releaseNoteResource = _mapper.Map<List<ReleaseNote>, List<ReleaseNoteResource>>(result.List);
            return Ok(releaseNoteResource);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = ("Administrator"))]
        public async Task<ActionResult<ReleaseNoteResource>> RemoveReleaseNote(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var result = await _releaseNoteService.RemoveReleaseNote(id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var releaseNoteResource = _mapper.Map<ReleaseNote, ReleaseNoteResource>(result.ReleaseNote);
            return Ok(releaseNoteResource);
        }
    }
}