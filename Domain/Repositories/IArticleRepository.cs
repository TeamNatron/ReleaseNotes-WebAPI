using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Utilities;

namespace ReleaseNotes_WebAPI.Domain.Repositories
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> ListAsync(ArticlesParameters queryParameters);
    }
}