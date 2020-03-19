using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReleaseNotes_WebAPI.Domain.Models.Auth;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Extensions;
using ReleaseNotes_WebAPI.Resources.Auth;

namespace ReleaseNotes_WebAPI.Controllers
{
    [Route("/api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = ("Administrator"))]
        public async Task<IActionResult> ListAsync()
        {
            var users = await _userService.ListAsync();
            var usersResource = _mapper.Map<IEnumerable<UserResource>>(users);
            return Ok(usersResource);
        }
        
        [HttpPost]
        [Authorize(Roles = ("Administrator"))]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserCredentialResource userCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<UserCredentialResource, User>(userCredentials);

            var response = await _userService.CreateUserAsync(user, ERole.Administrator);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            var userResource = _mapper.Map<User, UserResource>(response.User);
            return Ok(userResource);
        }
    }
}