using System.ComponentModel.DataAnnotations;

namespace ReleaseNotes_WebAPI.Domain.Models
{
    public class ProductVersion
    {
        public int Id { get; set; }
        
        [Required]
        public Product Product { get; set; }
        
        [Required]
        public string Version { get; set; }
        
        [Required]
        public bool IsPublic { get; set; }
    }
}