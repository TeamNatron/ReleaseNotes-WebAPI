using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Domain.Services.Communication
{
    public class ReleaseResponse : BaseResponse
    {

        public Release Release { get; private set; }
        
        public ReleaseResponse(bool success, string message, Release release) : base(success, message)
        {
            Release = release;
        }
        
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="release">Saved release.</param>
        /// <returns>Response.</returns>
        public ReleaseResponse(Release release) : this(true, string.Empty, release)
        {
        }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public ReleaseResponse(string message) : this(false, message, null)
        {
        }
    }
}