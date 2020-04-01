using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReleaseNotes_WebAPI.Domain.Models.Auth;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Domain.Services.Communication;
using ReleaseNotes_WebAPI.Resources;
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

        [Route("/api/users/azure")]
        [HttpGet]
        [Authorize(Roles = ("Administrator"))]
        public async Task<IActionResult> GetAzureInformation()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserEmail = User.FindFirst(claim =>
                claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var currentUser = await _userService.FindByEmailAsync(currentUserEmail);

            if (currentUser != null)
            {
                return Ok(currentUser.AzureInformation);
            }

            return NotFound();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = ("Administrator"))]
        public async Task<IActionResult> ChangeUserPassword(int id, [FromBody] UpdateUserPasswordResource
            updateUserPasswordResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.FindByIdAsync(id);
            var response = await _userService.ChangeUserPasswordAsync(user, updateUserPasswordResource.Password);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);
        }

        [HttpPut]
        [Authorize(Roles = ("Administrator"))]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserResource updateUserResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserEmail = User.FindFirst(claim =>
                claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

            var currentUser = await _userService.FindByEmailAsync(currentUserEmail);
            var response = await _userService.UpdateUserAsync(currentUser, updateUserResource);

            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            var userResource = _mapper.Map<User, UserDetailedResource>(response.User);
            return Ok(userResource);
        }
    }
}