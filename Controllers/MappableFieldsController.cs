using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Domain.Services.Communication;
using ReleaseNotes_WebAPI.Resources;
using ReleaseNotes_WebAPI.Utilities;

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
            var result = await _mappableService.ListAsync(mapped);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
    }
}