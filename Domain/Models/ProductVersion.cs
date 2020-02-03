using System.ComponentModel.DataAnnotations;

namespace ReleaseNotes_WebAPI.Domain.Models
{
    public class ProductVersion
    {
        public int Id { get; set; }
        
        public Product Product { get; set; }
        
        public string Version { get; set; }
        
        public bool IsPublic { get; set; }
    }
}