using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Domain.Services;

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

        public void AddAsync(ProductVersion productVersion)
        {
            _productVersionRepository.AddAsync(productVersion);
        }
    }
}