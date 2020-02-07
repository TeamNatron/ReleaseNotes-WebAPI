using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Resources;

namespace ReleaseNotes_WebAPI.Controllers
{
    [Route("/api/[controller]")]
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;

        public ArticlesController(IArticleService articleService, IMapper mapper)
        {
            _articleService = articleService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ArticleResource>> GetAllAsync([FromQuery(Name = "product")]int product)
        {
            if (product != null && product > 0) 
            { 
                var articlesByProductId = await _articleService.ListByProductAsync(product);
                return _mapper.Map<IEnumerable<ArticleResource>>(articlesByProductId);
            }
            var articles = await _articleService.ListAsync();
            var resource = _mapper.Map<IEnumerable<ArticleResource>>(articles);

            return resource;
        }
        
    }
}