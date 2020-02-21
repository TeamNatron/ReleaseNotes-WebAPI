using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Domain.Services.Communication;
using ReleaseNotes_WebAPI.Extensions;
using ReleaseNotes_WebAPI.Resources;

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
        [Authorize(Roles = "Administrator")]
        public async Task<IEnumerable<ReleaseNoteResource>> GetAllAsync()
        {
            var releaseNotes = await _releaseNoteService.ListAsync();
            var resources = _mapper.Map<IEnumerable<ReleaseNoteResource>>(releaseNotes);
            return resources;
        }

        // GET: api/releasenote/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ReleaseNoteResource>> GetSpecificReleaseNote(int id)
        {
            var releaseNoteResponse = await _releaseNoteService.GetReleaseNote(id);
            if (releaseNoteResponse == null)
            {
                return NotFound();
            }

            var releaseNote = _mapper.Map<ReleaseNote, ReleaseNoteResource>(releaseNoteResponse.ReleaseNote);
            return Ok(releaseNote);
        }

        [HttpPut]
        [Authorize(Roles = ("Administrator"))]
        public async Task<ActionResult<ReleaseNoteResource>> UpdateReleaseNoteAsync(
            [FromBody] EditReleaseNoteResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var result = await _releaseNoteService.UpdateReleaseNote(resource);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var releaseNoteResource = _mapper.Map<ReleaseNote, ReleaseNoteResource>(result.ReleaseNote);
            return Ok(releaseNoteResource);
        }
    }
}