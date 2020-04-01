using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReleaseNotes_WebAPI.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }
        
        public bool IsPublic { get; set; }
        
        public List<ProductVersion> ProductVersions { get; set; }
    }
}