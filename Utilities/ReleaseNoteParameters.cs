using System;

namespace ReleaseNotes_WebAPI.Utilities
{
    // https://code-maze.com/filtering-aspnet-core-webapi/
    public class ReleaseNoteParameters
    {
            // TODO: confirm that startDate and endDate are null when not specifed.
            public DateTime? StartDate { get; set; } = null;
            // NB! endDate sent by frontend is Now.byDefault
            public DateTime? EndDate { get; set; } = null;
    }
}