using System.ComponentModel.DataAnnotations;

namespace ReleaseNotes_WebAPI.Domain.Models
{
    public class ProductVersion
    {
        public int Id { get; set; }
        
        // Needed to automatically configure One-To-One relation.
        // https://www.entityframeworktutorial.net/efcore/one-to-one-conventions-entity-framework-core.aspx
        public int ProductId { get; set; }
        
        public Product Product { get; set; }
        
        public string Version { get; set; }
        
        public bool IsPublic { get; set; }
    }
}