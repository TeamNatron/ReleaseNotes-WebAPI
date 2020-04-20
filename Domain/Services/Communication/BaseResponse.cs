using Newtonsoft.Json;

namespace ReleaseNotes_WebAPI.Domain.Services.Communication
{
    public abstract class BaseResponse
    {
        [JsonIgnore]
        public bool Success { get; protected set; }
        public string Message { get; protected set; }

        public BaseResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}