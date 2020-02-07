using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Domain.Services
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> ListAsync();
        Task<IEnumerable<Article>> ListByProductAsync(int pid);
    }
}