using System.Collections.Generic;
using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Domain.Services.Communication
{
    public class ReleaseNoteResponse : BaseResponse
    {
        public ReleaseNote ReleaseNote { get; set; }

        public ReleaseNoteResponse(bool success, string message, ReleaseNote releaseNote) : base(success, message)
        {
            ReleaseNote = releaseNote;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="releaseNote">Saved release note.</param>
        /// <returns>Response.</returns>
        public ReleaseNoteResponse(ReleaseNote releaseNote) : this(true, "Release note created!", releaseNote)
        {
        }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public ReleaseNoteResponse(string message) : this(false, "Error occurred! " + message, null)
        {
        }
    }
}