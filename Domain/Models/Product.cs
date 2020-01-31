using System.ComponentModel.DataAnnotations;

namespace ReleaseNotes_WebAPI.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        
        [Required]
        public bool IsPublic { get; set; }
    }
}