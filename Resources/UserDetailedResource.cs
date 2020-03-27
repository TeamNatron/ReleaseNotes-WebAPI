using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Resources.Auth;

namespace ReleaseNotes_WebAPI.Resources
{
    public class UserDetailedResource : UserResource
    {
        public AzureInformation AzureInformation { get; set; }
    }
}