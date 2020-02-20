namespace ReleaseNotes_WebAPI.Resources
{
    public class EditReleaseNoteResource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Ingress { get; set; }
        public string DetailedView { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public string WorkItemTitle { get; set; }
        public bool IsPublic { get; set; }
    }
}