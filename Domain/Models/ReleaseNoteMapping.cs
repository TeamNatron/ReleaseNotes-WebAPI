namespace ReleaseNotes_WebAPI.Domain.Models
{
    public class ReleaseNoteMapping
    {
        public string AzureDevOpsField { get; set; }
        
        public int MappableFieldId { get; set; }
        
        public MappableField MappableField { get; set; }
        
        
        public int MappableTypeId { get; set; }

        public MappableType MappableType { get; set; }
    }
}