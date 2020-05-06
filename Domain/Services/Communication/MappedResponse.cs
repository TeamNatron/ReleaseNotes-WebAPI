using System;
using System.Collections.Generic;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Resources;

namespace ReleaseNotes_WebAPI.Domain.Services.Communication
{
    public class MappedResponse : BaseResponse
    {
        public MappedResponse(bool success, string message) : base(success, message)
        {
        }

        public IEnumerable<ReleaseNoteMappingResource> Entity { get; }
        
        public MappedResponse(bool success, string message, IEnumerable<ReleaseNoteMappingResource> entity) : base(success, message)
        {
            Entity = entity;
        }
    }
}