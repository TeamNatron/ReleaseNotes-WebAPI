namespace ReleaseNotes_WebAPI.Domain.Services.Communication
{
    public class ImageResponse : BaseResponse
    {
        public string Url { get; private set; }
        public ImageResponse(bool success, string message) : base(success, message)
        {
        }

        public ImageResponse(bool success, string message, string url) : base(success, message)
        {
            Url = url;
        }
        public ImageResponse(string url) : this(true, string.Empty, url)
        {
        }
    }
}