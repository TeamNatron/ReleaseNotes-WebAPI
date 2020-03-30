using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Domain.Services.Communication;

namespace ReleaseNotes_WebAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductVersionRepository _productVersionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork,
            IProductVersionRepository productVersionRepository)
        {
            _productRepository = productRepository;
            _productVersionRepository = productVersionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> ListAsync()
        {
            return await _productRepository.ListAsync();
        }

        public async Task<ProductResponse> SaveAsync(Product product)
        {
            try
            {
                var existingProduct = await _productRepository.FindByNameAsync(product.Name);

                if (existingProduct != null)
                {
                    return new ProductResponse("Produkt med navnet: " + product.Name + " finnes allerede i databasen.");
                }

                var defaultVersion = new ProductVersion {Product = product, Version = "1.0", IsPublic = product.IsPublic};
                await _productRepository.AddAsync(product);
                _productVersionRepository.AddAsync(defaultVersion);
                await _unitOfWork.CompleteAsync();

                return new ProductResponse(product);
            }
            catch (Exception e)
            {
                return new ProductResponse(
                    $"En feil oppsto når førsøkte å opprette nytt produkt: {e.Message}");
            }
        }
    }
}