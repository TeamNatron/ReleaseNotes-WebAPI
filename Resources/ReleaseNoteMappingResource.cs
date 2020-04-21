using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Resources
{
    public class ReleaseNoteMappingResource
    {
        public int Id { get; set; }
        public string AzureDevOpsField { get; set; }
        public string MappableField { get; set; }
        public string MappableType { get; set; }
    }
}