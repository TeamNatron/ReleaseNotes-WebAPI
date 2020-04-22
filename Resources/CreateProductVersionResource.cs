namespace ReleaseNotes_WebAPI.Resources
{
    public class CreateProductVersionResource
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public bool IsPublic { get; set; }
    }
}