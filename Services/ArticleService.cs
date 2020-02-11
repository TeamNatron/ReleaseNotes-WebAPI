using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Utilities;

namespace ReleaseNotes_WebAPI.Services
{
    public class ArticleService : IArticleService
    {

        private readonly IArticleRepository _articleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ArticleService(IArticleRepository articleRepository, IUnitOfWork unitOfWork)
        {
            _articleRepository = articleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Article>> ListAsync(ArticlesParameters queryParameters)
        {
            return await _articleRepository.ListAsync(queryParameters);
        }
    }
}