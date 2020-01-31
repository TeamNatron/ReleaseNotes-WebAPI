using System.ComponentModel.DataAnnotations;

namespace ReleaseNotes_WebAPI.Domain.Models
{
    public class Release
    {
        public int Id { get; set; }
        
        [Required]
        public ProductVersion ProductVersion { get; set; }
        
        public Article Article { get; set; }
        
        [Required]
        public bool IsPublic { get; set; }
    }
}