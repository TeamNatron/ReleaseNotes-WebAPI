using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Utilities;

namespace ReleaseNotes_WebAPI.Domain.Services
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> ListAsync(ArticlesParameters queryParameters);
    }
}