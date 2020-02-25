using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Resources;

namespace ReleaseNotes_WebAPI.Controllers
{
    [Route("/api/[controller]")]
    public class ProductVersionsController : Controller
    {
        private readonly IProductVersionService _productVersionService;
        private readonly IMapper _mapper;

        public ProductVersionsController(IProductVersionService productVersionService, IMapper mapper)
        {
            _productVersionService = productVersionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductVersionResource>> GetAllAsync()
        {
            var productsVersions = await _productVersionService.ListAsync();
            var resources = _mapper.Map<IEnumerable<ProductVersionResource>>(productsVersions);

            return resources;
        }
    }
}