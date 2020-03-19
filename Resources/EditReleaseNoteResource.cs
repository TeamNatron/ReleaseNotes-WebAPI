using System.ComponentModel.DataAnnotations;

namespace ReleaseNotes_WebAPI.Resources
{
    public class EditReleaseNoteResource
    {    
        public string Title { get; set; }
        public string Ingress { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsPublic { get; set; }
    }
}