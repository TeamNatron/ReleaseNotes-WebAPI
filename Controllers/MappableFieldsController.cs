using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Resources;

namespace ReleaseNotes_WebAPI.Controllers
{
    [Route("/api/[controller]")]
    public class MappableFieldsController: Controller
    {
        private readonly IMapper _mapper;
        private readonly IMappableService _mappableService;

        public MappableFieldsController(IMapper mapper, IMappableService mappableService)
        {
            _mapper = mapper;
            _mappableService = mappableService;
        }
        
        [HttpGet]
        [Authorize(Roles = ("Administrator"))]
        public async Task<IActionResult> ListAsync([FromQuery] bool mapped)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _mappableService.ListAsync(mapped);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
        
        [HttpPut("{id}")]
        [Authorize(Roles = ("Administrator"))]
        public async Task<IActionResult> UpdateReleaseNoteMappingAsync(
            int id, [FromBody] UpdateReleaseNoteMappingResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _mappableService.UpdateReleaseNoteMappingAsync(resource, id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
    }
}