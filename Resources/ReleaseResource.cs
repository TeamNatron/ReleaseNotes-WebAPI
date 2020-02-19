using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Resources
{
    public class ReleaseResource
    {
        public int Id { get; set; }
        
        public ProductVersion ProductVersion { get; set; }
        
        public string Title { get; set; }
        
        public bool IsPublic { get; set; }
        
        public ICollection<ReleaseNote> ReleaseNotes { get; set; } = new Collection<ReleaseNote>();
    }
}