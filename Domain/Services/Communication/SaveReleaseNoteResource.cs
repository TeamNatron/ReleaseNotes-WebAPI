using System;
using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Domain.Services.Communication
{
    public class SaveReleaseNoteResource
    {
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