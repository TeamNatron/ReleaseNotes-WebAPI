using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Domain.Services.Communication;
using ReleaseNotes_WebAPI.Resources;
using ReleaseNotes_WebAPI.Utilities;

namespace ReleaseNotes_WebApi.Services
{
    public class MappableService : IMappableService
    {
        private readonly IMappableRepository _mappableRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public MappableService(IMappableRepository mappableRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mappableRepository = mappableRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MappableResponse> ListMappableAsync()
        {
            var result = await _mappableRepository.GetMappableFields();

            var mappableFields = _mapper.Map<IEnumerable<MappableFieldsResource>>(result);
            var response = new MappableResponse(
                true, "Innhenting av Mapped Fields er vellykket", mappableFields);

            return response;
        }


        public async Task<MappedResponse> ListMappedAsync(string type)
        {
            var result = await _mappableRepository.GetMappedFields(type);

            var releaseNoteMappings = _mapper.Map<IEnumerable<ReleaseNoteMappingResource>>(result);
            var response = new MappedResponse(
                true, "Innhenting av Release Note Mappings er vellykket", releaseNoteMappings);

            return response;
        }

        public async Task<MappingResponse> UpdateReleaseNoteMappingAsync(UpdateReleaseNoteMappingResource resource,
            string type, string mappableField)
        {
            var existingMapping = await _mappableRepository.FindAsync(type, mappableField);

            if (existingMapping == null)
            {
                return new MappingResponse(false, "Mapping finnes ikke!");
            }

            try
            {
                _mapper.Map(resource, existingMapping);
                _mappableRepository.UpdateReleaseNoteMappingAsync(existingMapping);
                await _unitOfWork.CompleteAsync();

                // This extra mapping ensures that only resources gets returned
                var result = _mapper.Map<ReleaseNoteMapping, ReleaseNoteMappingResource>(existingMapping);
                return new MappingResponse(true, "Oppdatering vellykket!", result);
            }
            catch (Exception e)
            {
                return new MappingResponse(false, $"Det oppsto en feil: {e.Message}");
            }
        }
    }
}