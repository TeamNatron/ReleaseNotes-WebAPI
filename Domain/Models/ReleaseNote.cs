using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReleaseNotes_WebAPI.Domain.Models
{
    public class ReleaseNote
    {
        public int Id { get; }

        public Release Release { get; set; }

        public int WorkItemId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorEmail { get; set; }

        public string WorkItemDescriptionHtml { get; set; }

        public string WorkItemTitle { get; set; }

        //[StringLength(512)]
        //public string Comment { get; set; }

        public DateTime ClosedDate { get; set; }

        public bool IsPublic { get; set; } = false;
    }
}