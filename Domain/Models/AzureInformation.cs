using ReleaseNotes_WebAPI.Domain.Models.Auth;

namespace ReleaseNotes_WebAPI.Domain.Models
{
    public class AzureInformation
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Pat { get; set; }
        public string Organization { get; set; }
    }
}