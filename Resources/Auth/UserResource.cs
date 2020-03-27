using System.Collections.Generic;
using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Resources.Auth
{
    public class UserResource
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public AzureInformation AzureInformation { get; set; }
    }
}