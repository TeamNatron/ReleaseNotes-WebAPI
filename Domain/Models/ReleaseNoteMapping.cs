namespace ReleaseNotes_WebAPI.Domain.Models
{
    public class ReleaseNoteMapping
    {
        public int Id { get; set; }
        
        public string AzureDevOpsField { get; set; }
        
        public int MappableFieldId { get; set; }
        
        public MappableField MappableField { get; set; }

    }
}