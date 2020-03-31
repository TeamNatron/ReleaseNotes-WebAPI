using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Resources
{
    public class SaveReleaseResource
    {
        [Required]
        public int ProductVersionId { get; set; }
        
        public string Title { get; set; }
        
        public bool IsPublic { get; set; }
        
        public IEnumerable<int> ReleaseNotesId { get; set; }
        
        public IEnumerable<ReleaseNote> ReleaseNotes { get; set; }
    }
}