using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Extensions;
using ReleaseNotes_WebAPI.Resources;
using ReleaseNotes_WebAPI.Utilities;

namespace ReleaseNotes_WebAPI.Controllers
{
    [Route("/api/[controller]")]
    public class ReleasesController : Controller
    {
        private readonly IReleaseService _releaseService;
        private readonly IMapper _mapper;

        public ReleasesController(IReleaseService releaseService, IMapper mapper)
        {
            _releaseService = releaseService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PostAsync([FromBody] SaveReleaseResource resource)
        {
            // Check if the user data works with this model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var result = await _releaseService.SaveAsync(resource);

            if (!result.Success) return BadRequest(result.Message);

            var releaseResource = _mapper.Map<Release, ReleaseResource>(result.Release);
            return Ok(releaseResource);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveReleaseResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var result = await _releaseService.UpdateAsync(id, resource);

            if (!result.Success) return BadRequest(result.Message);

            var releaseResource = _mapper.Map<Release, ReleaseResource>(result.Release);
            return Ok(releaseResource);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ReleaseResource>> GetAsync(int id)
        {
            var releaseResponse = await _releaseService.GetRelease(id);
            if (releaseResponse == null)
            {
                return NotFound();
            }

            var release = _mapper.Map<Release, ReleaseResource>(releaseResponse.Release);
            return Ok(release);
        }

        [HttpGet]
        public async Task<IEnumerable<ReleaseResource>> ListAsync([FromQuery] ReleasesParameters queryParameters)
        {
            var result = await _releaseService.ListAsync(queryParameters);
            var releasesResource = _mapper.Map<IEnumerable<ReleaseResource>>(result);
            return releasesResource;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ReleaseResource>> RemoveRelease(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var result = await _releaseService.RemoveRelease(id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var releaseResource = _mapper.Map<Release, ReleaseResource>(result.Release);
            return Ok(releaseResource);

        }

    }
}