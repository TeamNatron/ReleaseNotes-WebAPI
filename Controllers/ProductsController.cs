using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Models.Auth;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Resources;
using ReleaseNotes_WebAPI.Resources.Auth;

namespace ReleaseNotes_WebAPI.Controllers
{
    [Route("/api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductVersionService _productVersionService;

        private readonly IMapper _mapper;

        public ProductsController(
            IProductService productService, 
            IProductVersionService productVersionService,
            IMapper mapper)
        {
            _productService = productService;
            _productVersionService = productVersionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductResource>> GetAllAsync()
        {
            var products = await _productService.ListAsync();
            var resources = _mapper.Map<IEnumerable<ProductResource>>(products);

            return resources;
        }

        [HttpPost]
        [Authorize(Roles = ("Administrator"))]
        public async Task<IActionResult> CreateProductAsync([FromBody] SaveProductResource productResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<SaveProductResource, Product>(productResource);

            var response = await _productService.SaveAsync(product);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            var userResource = _mapper.Map<Product, ProductResource>(response.Product);
            return Ok(userResource);
        }

        [HttpPost]
        [Route("{id}/version")]
        public async Task<IActionResult> AddVersionAsync(int id, [FromBody] CreateProductVersionResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newProductVersion = _mapper.Map<ProductVersion>(resource);
            newProductVersion.ProductId = id;
            var result = await _productVersionService.AddAsync(newProductVersion);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            var response = _mapper.Map<CreateProductVersionResource>(result.ProductVersion);
            return Ok(response);
        }
    }
}