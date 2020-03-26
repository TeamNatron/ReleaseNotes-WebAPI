using System.ComponentModel.DataAnnotations;
using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Resources.Auth
{
    public class UpdateUserResource
    {
        [Required] [MinLength(5)] public string Password { get; set; }
        public AzureInformation AzureInformation { get; set; }
    }
}