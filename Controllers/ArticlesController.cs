using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Resources;
using ReleaseNotes_WebAPI.Utilities;

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
        public async Task<IEnumerable<ArticleResource>> GetAllAsync([FromQuery] ArticlesParameters queryParameters)
        {
            var articles = await _articleService.ListAsync(queryParameters);
            var resource = _mapper.Map<IEnumerable<ArticleResource>>(articles);
            return resource;
        }
        
    }
}