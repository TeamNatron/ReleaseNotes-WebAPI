using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Resources
{
    public class ReleaseNoteMappingResource
    {
        public int Id { get; set; }
        
        public string AzureDevOpsField { get; set; }
        
        public int MappableFieldId { get; set; }
        
        public MappableField MappableField { get; set; }
    }
}