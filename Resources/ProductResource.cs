using System.Collections.Generic;

namespace ReleaseNotes_WebAPI.Resources
{
    public class ProductResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductVersionResource> Versions { get; set; }
    }
}