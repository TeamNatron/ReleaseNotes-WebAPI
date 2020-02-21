using System;
using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Resources
{
    public class ReleaseNoteResource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Ingress { get; set; }
        public string DetailedView { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public string WorkItemDescriptionHtml { get; set; }
        public string WorkItemTitle { get; set; }
        public DateTime ClosedDate { get; set; }
        public bool IsPublic { get; set; }
    }
}