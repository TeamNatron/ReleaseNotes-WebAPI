using System.ComponentModel.DataAnnotations;

namespace ReleaseNotes_WebAPI.Resources.Auth
{
    public class RevokeTokenResource
    {
        [Required] public string Token { get; set; }
    }
}