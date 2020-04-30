using System.Collections.Generic;
using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Resources
{
    public class MappableFieldsResource
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string DataType { get; set; }
    }
}