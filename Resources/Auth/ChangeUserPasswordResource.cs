using System.ComponentModel.DataAnnotations;
using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Resources.Auth
{
    public class ChangeUserPasswordResource
    {
        [Required] [MinLength(5)] public string Password { get; set; }
    }
}