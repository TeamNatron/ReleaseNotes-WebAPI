using System.ComponentModel;

namespace ReleaseNotes_WebAPI.Domain.Models.Auth
{
    public enum ERole
    {
        [Description("Common")] Common = 1,
        [Description("Administrator")] Administrator = 2
    }
}