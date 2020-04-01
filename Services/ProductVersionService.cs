using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Domain.Services.Communication;

namespace ReleaseNotes_WebAPI.Services
{
    public class ProductVersionService : IProductVersionService
    {
        private readonly IProductVersionRepository _productVersionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductVersionService(IProductVersionRepository productVersionRepository, IUnitOfWork unitOfWork)
        {
            _productVersionRepository = productVersionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductVersion>> ListAsync()
        {
            return await _productVersionRepository.ListAsync();
        }

        public async Task<ProductVersionResponse> AddAsync(ProductVersion productVersion)
        {
            var exists = await _productVersionRepository.AnyAsync(productVersion);
            if (exists)
            {
                return new ProductVersionResponse(false, "Produktet har allerede denne versjonen!");
            }
            await _productVersionRepository.AddAsync(productVersion);
            await _unitOfWork.CompleteAsync();
            return new ProductVersionResponse(true, "Opprettet", productVersion);
        }
    }
}