using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Models.Auth;
using ReleaseNotes_WebAPI.Domain.Models.Auth.Token;
using ReleaseNotes_WebAPI.Domain.Services.Communication;
using ReleaseNotes_WebAPI.Resources;
using ReleaseNotes_WebAPI.Resources.Auth;

namespace ReleaseNotes_WebAPI.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<User, UserResource>()
                .ForMember(u => u.Roles, opt => opt.MapFrom(u => u.UserRoles.Select(ur => ur.Role.Name)));

            CreateMap<AccessToken, AccessTokenResource>()
                .ForMember(
                    a => a.AccessToken,
                    opt => opt.MapFrom(
                        a => a.Token))
                .ForMember(a => a.RefreshToken,
                    opt => opt.MapFrom(
                        a => a.RefreshToken.Token))
                .ForMember(a => a.Expiration,
                    opt => opt.MapFrom(
                        a => a.Expiration));

            CreateMap<Article, ArticleResource>();
            CreateMap<Release, ReleaseResource>();
            CreateMap<ProductVersion, ProductVersionResource>();
            CreateMap<Product, ProductResource>();
            CreateMap<ReleaseNote, ReleaseNoteResource>();
        }
    }
}