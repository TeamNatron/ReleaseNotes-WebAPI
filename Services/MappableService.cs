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

        public async Task<MappingResponse> ListAsync(bool mapped)
        {
            dynamic result;
            if (mapped)
            {
                result =  await _mappableRepository.GetMappedFields();
            }
            else
            {
                result = await _mappableRepository.GetMappableFields();
            }

            // The response will start out as a failed attempt to retrieve data, this is because
            // C#'s compiler doesn't recognize the use of default when using the TypeSwitch class..
            var response = new MappingResponse(
                false, "Noe gikk galt, vennligst kontakt din lokale utvikler..");
            
            TypeSwitch.Do(
                result,
                TypeSwitch.Case<IEnumerable<MappableField>>(() =>
                {
                    var mappableFields = _mapper.Map<IEnumerable<MappableFieldsResource>>(result);
                    response =  new MappingResponse(
                        true, "Innhenting av Mapped Fields er vellykket", mappableFields);
                }),
                TypeSwitch.Case<IEnumerable<ReleaseNoteMapping>>(() =>
                {
                    var releaseNoteMappings = _mapper.Map<IEnumerable<ReleaseNoteMapping>>(result);
                    response = new MappingResponse(
                        true, "Innhenting av Release Note Mappings er vellykket", releaseNoteMappings);
                })
            );
            
            return response;
        }
    }
}