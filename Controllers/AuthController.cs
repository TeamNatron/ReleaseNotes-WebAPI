using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReleaseNotes_WebAPI.Domain.Models.Auth.Token;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Extensions;
using ReleaseNotes_WebAPI.Resources.Auth;
using ReleaseNotes_WebAPI.Utilities;

namespace ReleaseNotes_WebAPI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public LoginController(IMapper mapper, IAuthenticationService authenticationService)
        {
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        [Route("/api/login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(
            [FromBody] UserCredentialResource userCredentialResource)
        {
            // Check if the user data works with this model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var response =
                await _authenticationService
                    .CreateAccessTokenAsync(userCredentialResource.Email,
                        userCredentialResource.Password);

            if (!response.Success)
                return BadRequest(response.Message);

            var accessTokenResource =
                _mapper.Map<AccessToken, AccessTokenResource>(response.Token);
            return Ok(accessTokenResource);
        }

        [Route("api/token/refresh")]
        [HttpPost]
        public async Task<IActionResult> RefreshTokenAsync(
            [FromBody] RefreshTokenResource refreshTokenResource)
        {
            // Check if the user data works with this model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var response =
                await _authenticationService.RefreshTokenAsync(refreshTokenResource.Token,
                    refreshTokenResource.UserEmail);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            var tokenResource = _mapper.Map<AccessToken, AccessTokenResource>(response.Token);
            return Ok(tokenResource);
        }

        [Route("api/token/revoke")]
        [HttpPost]
        public IActionResult RevokeToken(
            [FromBody] RevokeTokenResource revokeTokenResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _authenticationService.RevokeRefreshToken(revokeTokenResource.Token);
            return NoContent();
        }

        [Route("api/publickey.pem")]
        [HttpGet]
        public async Task<String> GetPublicKey()
        {
            return await FileHandler.ReadTextFileAsync("", "publickey.txt");
        }
    }
}