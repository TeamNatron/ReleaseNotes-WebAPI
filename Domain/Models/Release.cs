using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReleaseNotes_WebAPI.Domain.Models
{
    public class Release
    {
        public int Id { get; set; }
        
        // Needed to automatically configure One-To-One relation.
        // https://www.entityframeworktutorial.net/efcore/one-to-one-conventions-entity-framework-core.aspx
        public int ProductVersionId { get; set; }
        
        public ProductVersion ProductVersion { get; set; }
        
        public Article Article { get; set; }
        
        public bool IsPublic { get; set; }
    }
}