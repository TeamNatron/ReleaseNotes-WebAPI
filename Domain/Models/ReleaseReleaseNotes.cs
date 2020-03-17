namespace ReleaseNotes_WebAPI.Domain.Models
{
    public class ReleaseReleaseNotes
    {
        public int ReleaseId { get; set; }
        public Release Release { get; set; }

        public int ReleaseNoteId { get; set; }
        public ReleaseNote ReleaseNote { get; set; }
    }
}