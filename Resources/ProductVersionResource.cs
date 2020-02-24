namespace ReleaseNotes_WebAPI.Resources
{
    public class ProductVersionResource
    {
        public int Id { get; set; }
        
        public ProductResource Product { get; set; }
        
        public string Version { get; set; }
    }
}