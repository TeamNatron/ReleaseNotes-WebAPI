using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using ReleaseNotes_WebAPI.Domain.Models.Auth;

namespace ReleaseNotes_WebAPI.Security
{
    public static class ClaimsUtil
    {
        public static bool CheckIfUserIsAdmin(IHttpContextAccessor accessor)
        {
            var claims = accessor.HttpContext.User.Claims.ToList();
            if (claims.Any())
            {
                var roleClaim = claims.SingleOrDefault(
                    claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                if (roleClaim != null)
                {
                    if (roleClaim.Value == Enum.GetName(typeof(ERole), ERole.Administrator))
                    {
                        return true;
                    } 
                }
            }
            return false;
        }
    }
}