using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Resources;

namespace ReleaseNotes_WebAPI.Controllers
{
    [Route("/api/[controller]")]
    public class MappableFieldsController : Controller
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
        public async Task<IActionResult> ListAsync([FromQuery] bool mapped, [FromQuery] string type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dynamic result;
            if (mapped)
            {
                result = await _mappableService.ListMappedAsync(type);
            }
            else
            {
                result = await _mappableService.ListMappableAsync();
            }

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPut("{type}/{mappableField}")]
        [Authorize(Roles = ("Administrator"))]
        public async Task<IActionResult> UpdateReleaseNoteMappingAsync(
            [FromBody] UpdateReleaseNoteMappingResource resource, string type, string mappableField)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mappableService.UpdateReleaseNoteMappingAsync(resource, type, mappableField);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
    }
}