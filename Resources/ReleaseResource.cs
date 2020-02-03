namespace ReleaseNotes_WebAPI.Resources
{
    public class ReleaseResource
    {
        public int Id { get; set; }
        
        public ProductVersionResource ProductVersion { get; set; }
    }
}