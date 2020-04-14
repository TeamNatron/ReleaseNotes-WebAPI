using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Domain.Services;

namespace ReleaseNotes_WebApi.Services
{
    public class MappableService : IMappableService
    {
        private readonly IMappableRepository _mappableRepository;
        private readonly IUnitOfWork _unitOfWork;


        public MappableService(IMappableRepository mappableRepository, IUnitOfWork unitOfWork)
        {
            _mappableRepository = mappableRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MappableField>> ListAsync()
        {
            return await _mappableRepository.ListAsync();
        }
    }
}