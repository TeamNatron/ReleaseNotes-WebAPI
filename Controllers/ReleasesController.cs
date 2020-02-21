using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Extensions;
using ReleaseNotes_WebAPI.Resources;

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
        [Authorize(Roles="Administrator")]
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
    }
}