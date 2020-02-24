using System.Collections.Generic;
using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Resources
{
    public class SaveReleaseResource
    {
        public int ProductVersionId { get; set; }
        
        public string Title { get; set; }
        
        public bool IsPublic { get; set; }
        
        public IEnumerable<int> ReleaseNotesId { get; set; }
    }
}