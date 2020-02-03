using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Persistence.Contexts;

namespace ReleaseNotes_WebAPI.Persistence.Repositories
{
    public class ArticleRepository : BaseRepository, IArticleRepository
    {
        public ArticleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Article>> ListAsync()
        {
            return await _context.Articles
                .Include(a => a.Release)
                .ThenInclude(r => r.ProductVersion)
                .ThenInclude(pr => pr.Product)
                .ToListAsync();
        }
    }
}