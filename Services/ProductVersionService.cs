using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProductVersionService(IProductVersionRepository productVersionRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _productVersionRepository = productVersionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public async Task<ProductVersionResponse> UpdateAsync(ProductVersion productVersion)
        {
            var existingProductVersion = await _productVersionRepository.FindAsync(productVersion);
            if (existingProductVersion == null)
            {
                return new ProductVersionResponse(false, "Finner ikke denne produktversjonen!");
            }

            // Map existing object
            if (productVersion.Version != null)existingProductVersion.Version = productVersion.Version;
            existingProductVersion.IsPublic = productVersion.IsPublic;

            try
            {
                // Persist update
                _productVersionRepository.Update(existingProductVersion);
                await _unitOfWork.CompleteAsync();
                
                // Return response with updated object
                return new ProductVersionResponse(true, "Oppdatert!", existingProductVersion);

            }
            catch (Exception e)
            {
                return new ProductVersionResponse(false, "Noe gikk fryktelig galt: " + e.Message);
            }
        }
    }
}