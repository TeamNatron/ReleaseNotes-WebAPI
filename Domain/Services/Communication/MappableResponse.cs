using System.Collections;
using System.Collections.Generic;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Resources;

namespace ReleaseNotes_WebAPI.Domain.Services.Communication
{
    public class MappableResponse : BaseResponse
    {
        public MappableResponse(bool success, string message) : base(success, message)
        {
        }

        public IEnumerable<MappableFieldsResource> Entity { get; }

        public MappableResponse(bool success, string message, IEnumerable<MappableFieldsResource> entity) : base(success, message)
        {
            Entity = entity;
        }
    }
}