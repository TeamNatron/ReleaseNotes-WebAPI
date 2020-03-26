using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Models.Auth;
using ReleaseNotes_WebAPI.Resources;
using ReleaseNotes_WebAPI.Resources.Auth;

namespace ReleaseNotes_WebAPI.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<UserCredentialResource, User>();
            CreateMap<ChangeUserPasswordResource, User>();
            CreateMap<ReleaseNoteResource, ReleaseNote>();
            CreateMap<EditReleaseNoteResource, ReleaseNote>().ForAllMembers(
                opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<CreateReleaseNoteResource, ReleaseNote>();
            CreateMap<SaveReleaseResource, Release>();
            CreateMap<SaveProductResource, Product>();
        }
    }
}