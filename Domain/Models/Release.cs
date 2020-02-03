using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReleaseNotes_WebAPI.Domain.Models
{
    public class Release
    {
        public int Id { get; set; }
        
        public int ProductVersionId { get; set; }
        
        public ProductVersion ProductVersion { get; set; }
        
        public Article Article { get; set; }
        
        public bool IsPublic { get; set; }
    }
}