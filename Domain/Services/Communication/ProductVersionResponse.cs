using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Domain.Services.Communication
{
    public class ProductVersionResponse : BaseResponse
    {
        public ProductVersionResponse(bool success, string message) : base(success, message)
        {
        }

        public ProductVersion ProductVersion { get; private set; }
        
        public ProductVersionResponse(bool success, string message, ProductVersion productVersion) : base(success, message)
        {
            ProductVersion = productVersion;
        }
        
        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public ProductVersionResponse(string message) : this(false, message, null)
        {
        }
    }
}