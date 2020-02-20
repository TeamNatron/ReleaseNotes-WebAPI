using System;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Domain.Services.Communication;
using ReleaseNotes_WebAPI.Resources;

namespace ReleaseNotesWebAPI.Services
{
    public class ReleaseService : IReleaseService
    {
        private readonly IReleaseRepository _releaseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReleaseService(IReleaseRepository releaseRepository, IUnitOfWork unitOfWork)
        {
            _releaseRepository = releaseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ReleaseResponse> SaveAsync(SaveReleaseResource resource)
        {
            try
            {
                var existingRelease = await _releaseRepository.FindByNameAsync(resource.Title);

                if (existingRelease != null)
                {
                    return new ReleaseResponse("Release med navnet: " + resource.Title + " finnes allerede i databasen.");
                }

                Release release;
                try
                {
                    release = new Release
                    {
                        Title = resource.Title,
                        IsPublic = resource.IsPublic,
                        ProductVersion = await _releaseRepository.FindProductVersion(resource.ProductVersionId),
                        ReleaseNotes = await _releaseRepository.FindReleaseNotes(resource.ReleaseNotesId)
                    };
                }
                catch (Exception e)
                {
                    return new ReleaseResponse(e.Message);
                }


                await _releaseRepository.AddAsync(release);
                await _unitOfWork.CompleteAsync();

                return new ReleaseResponse(release);
            }
            catch (Exception e)
            {
                return new ReleaseResponse(
                    $"En feil oppsto når førsøkte å opprette nytt produkt: {e.Message}");
            }
        }
    }
}