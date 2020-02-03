namespace ReleaseNotes_WebAPI.Resources
{
    public class ArticleResource
    {
        public int Id { get; set; }
        
        public string Uri { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public ReleaseResource Release { get; set; }
    }
}