using System;

namespace ReleaseNotes_WebAPI.Domain.Services.Communication
{
    public class MappingResponse : BaseResponse
    {
        public MappingResponse(bool success, string message) : base(success, message)
        {
        }

        public object Entity { get; }
        
        public MappingResponse(bool success, string message, object entity) : base(success, message)
        {
            Entity = entity;
        }
    }
}