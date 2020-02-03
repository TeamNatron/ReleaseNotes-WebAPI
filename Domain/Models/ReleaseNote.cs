using System.ComponentModel.DataAnnotations;

namespace ReleaseNotes_WebAPI.Domain.Models
{
    public class ReleaseNote
    {
        public int Id { get; set; }
        
        public Release Release { get; set; }
        
        [StringLength(512)]
        public string Comment { get; set; }
        
        public bool IsPublic { get; set; }
    }
}