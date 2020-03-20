using System.ComponentModel.DataAnnotations;

namespace ReleaseNotes_WebAPI.Resources.Auth
{
    public class UpdateUserPasswordResource
    {
        [Required] [MinLength(5)] public string Password { get; set; }
    }
}