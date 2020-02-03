using System.ComponentModel.DataAnnotations;

namespace ReleaseNotes_WebAPI.Domain.Models
{
    public class Article
    {
        public int Id { get; set; }

        // Needed to automatically configure One-To-One relation.
        // https://www.entityframeworktutorial.net/efcore/one-to-one-conventions-entity-framework-core.aspx
        public int ReleaseId { get; set; }
        public Release Release { get; set; }
        public string Uri { get; set; }
        public bool IsPublic { get; set; }
    }
}