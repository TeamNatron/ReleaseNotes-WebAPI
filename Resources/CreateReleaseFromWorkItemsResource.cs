using Newtonsoft.Json.Linq;

namespace ReleaseNotes_WebAPI.Resources
{
    public class CreateReleaseFromWorkItemsResource
    {
        public string Title { get; set; }
        public bool IsPublic { get; set; }
        public int ProductVersionId { get; set; }
        public JArray ReleaseNotes { get; set; }
        
    }
}