namespace ReleaseNotes_WebAPI.Domain.Models
{
    public class Article
    {
        public int Id { get; set; }
        
        public Release Release { get; set; }
        
        public string Uri { get; set; }
    }
}