using System.Collections.Generic;
using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Domain.Services.Communication
{
    public class ReleaseNotesResponse : BaseResponse
    {
        public List<ReleaseNote> List { get; set; }

        public ReleaseNotesResponse(bool success, string message, List<ReleaseNote> notes) : base(success, message)
        {
            List = notes;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="releaseNote">Saved a list that contains release notes.</param>
        /// <returns>Response.</returns>
        public ReleaseNotesResponse(List<ReleaseNote> notes) : this(true, "Release note array created!", notes)
        {
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public ReleaseNotesResponse(string message) : this(false, "Error occurred! " + message, null)
        {
        }
    }
}