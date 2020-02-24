using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Persistence.Contexts;
using ReleaseNotes_WebAPI.Utilities;

namespace ReleaseNotes_WebAPI.Persistence.Repositories
{
    public class ArticleRepository : BaseRepository, IArticleRepository
    {
        public ArticleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Article>> ListAsync(ArticlesParameters queryParameters)
        {
            IQueryable<Article> articlesQuery = _context.Articles
                .Include(a => a.Release)
                .ThenInclude(r => r.ProductVersion)
                .ThenInclude(pr => pr.Product);
            
            if (queryParameters.product > 0)
            {
                articlesQuery = articlesQuery.Where(a => a.Release.ProductVersion.Product.Id == queryParameters.product);
            }

            if (!String.IsNullOrEmpty(queryParameters.sort))
            {
                switch (queryParameters.sort)
                {
                    case "old":
                        articlesQuery = articlesQuery.OrderBy(a => a.Date);
                        break;
                    case "new":
                        articlesQuery = articlesQuery.OrderByDescending(a => a.Date);
                        break;
                }
            }
    
            return await articlesQuery.ToListAsync();
        }
    }
}