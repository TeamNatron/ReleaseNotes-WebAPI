using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReleaseNotes_WebAPI.Domain.Models
{
    public class ReleaseNote
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Ingress { get; set; }

        public string DetailedView { get; set; }

        public int WorkItemId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorEmail { get; set; }

        public string WorkItemDescriptionHtml { get; set; }

        public string WorkItemTitle { get; set; }

        public DateTime ClosedDate { get; set; }

        public bool IsPublic { get; set; } = false;
    }
}