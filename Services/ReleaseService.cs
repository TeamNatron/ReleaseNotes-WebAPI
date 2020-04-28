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

namespace ReleaseNotesWebAPI.Services
{
    public class ReleaseService : IReleaseService
    {
        private readonly IReleaseRepository _releaseRepository;
        private readonly IReleaseNoteService _releaseNoteService;
        private readonly IUnitOfWork _unitOfWork;

        public ReleaseService(IReleaseRepository releaseRepository, IReleaseNoteService releaseNoteService,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _releaseRepository = releaseRepository;
            _releaseNoteService = releaseNoteService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ReleaseResponse> GetRelease(int id)
        {
            var existingRelease = await _releaseRepository.FindByIdAsync(id);
            if (existingRelease == null)
            {
                return new ReleaseResponse("Releasen eksisterer ikke!");
            }

            try
            {
                return new ReleaseResponse(existingRelease);
            }
            catch (Exception e)
            {
                return new ReleaseResponse($"Det oppsto en feil: {e.Message}");
            }
        }

        public async Task<ReleaseResponse> CreateFromWorkItems(CreateReleaseFromWorkItemsResource resource)
        {
            var existingRelease = await _releaseRepository.FindByNameAsync(resource.Title);

            if (existingRelease != null)
            {
                return new ReleaseResponse(
                    "Release med navnet: " + resource.Title + " finnes allerede i databasen.");
            }

            Release release;
            try
            {
                // Create new Release entity
                release = new Release
                {
                    Title = resource.Title,
                    IsPublic = resource.IsPublic,
                    ProductVersion = await _releaseRepository.FindProductVersion(resource.ProductVersionId),
                };

                // Create new ReleaseReleaseNotes entity
                var releaseReleaseNotes = new List<ReleaseReleaseNote>();
                var releaseNotes = await _releaseNoteService.CreateReleaseNotesFromMap(resource.ReleaseNotes);
                // Map each ReleaseNote to this Release
                foreach (var releaseNote in releaseNotes.List)
                {
                    releaseReleaseNotes.Add(new ReleaseReleaseNote {Release = release, ReleaseNote = releaseNote});
                }

                // Add ReleaseReleaseNote to the Release
                release.ReleaseReleaseNotes = releaseReleaseNotes;
            }
            catch (Exception e)
            {
                return new ReleaseResponse(e.Message);
            }


            await _releaseRepository.AddAsync(release);
            await _unitOfWork.CompleteAsync();

            return new ReleaseResponse(release);
        }

        public async Task<ReleaseResponse> UpdateAsync(int id, SaveReleaseResource resource)
        {
            var existingRelease = await _releaseRepository.FindByIdAsync(id);

            if (existingRelease == null)
            {
                return new ReleaseResponse("Release med id " + id + " ikke funnet i databasen.");
            }

            if (resource.Title != null) existingRelease.Title = resource.Title;
            if (resource.IsPublic != null) existingRelease.IsPublic = resource.IsPublic;
            if (resource.ReleaseNotesId != null)
            {
                // Retrieve all ReleaseNotes from database
                var releaseNotes = await _releaseRepository.FindReleaseNotes(resource.ReleaseNotesId);

                // Remove all items from existing list
                existingRelease.ReleaseReleaseNotes.Clear();

                // Map each ReleaseNote to this Release
                foreach (var releaseNote in releaseNotes)
                {
                    existingRelease.ReleaseReleaseNotes.Add(new ReleaseReleaseNote
                        {Release = existingRelease, ReleaseNote = releaseNote});
                }
            }

            if (resource.ProductVersionId > 0 && resource.ProductVersionId != existingRelease.ProductVersionId)
            {
                var productVersion = await _releaseRepository.FindProductVersion(resource.ProductVersionId);
                if (productVersion != null)
                {
                    existingRelease.ProductVersion = productVersion;
                }
            }

            try
            {
                _releaseRepository.Update(existingRelease);
                await _unitOfWork.CompleteAsync();

                return new ReleaseResponse(existingRelease);
            }
            catch (Exception e)
            {
                return new ReleaseResponse($"En feil oppsto ved oppdatering av en release: {e.Message}");
            }
        }

        public async Task<IEnumerable<Release>> ListAsync(ReleasesParameters queryParameters)
        {
            return await _releaseRepository.ListAsync(queryParameters);
        }

        public async Task<ReleaseResponse> RemoveRelease(int id)
        {
            var existingRelease = await _releaseRepository.FindByIdAsync(id);
            if (existingRelease == null)
            {
                return new ReleaseResponse("Releasen finnes ikke!");
            }

            try
            {
                _releaseRepository.Remove(existingRelease);
                await _unitOfWork.CompleteAsync();
                return new ReleaseResponse(existingRelease);
            }
            catch (Exception e)
            {
                return new ReleaseResponse($"Det oppsto en feil: {e.Message}");
            }
        }

        public async Task<ReleaseResponse> SaveAsync(SaveReleaseResource resource)
        {
            try
            {
                var existingRelease = await _releaseRepository.FindByNameAsync(resource.Title);

                if (existingRelease != null)
                {
                    return new ReleaseResponse(
                        "Release med navnet: " + resource.Title + " finnes allerede i databasen.");
                }

                Release release;
                try
                {
                    // Create new Release entity
                    release = new Release
                    {
                        Title = resource.Title,
                        IsPublic = resource.IsPublic,
                        ProductVersion = await _releaseRepository.FindProductVersion(resource.ProductVersionId),
                    };

                    // Create new ReleaseReleaseNotes entity
                    var releaseReleaseNotes = new List<ReleaseReleaseNote>();

                    // Retrieve all ReleaseNotes from database
                    IEnumerable<ReleaseNote> releaseNotes;

                    if (resource.ReleaseNotesId != null)
                    {
                        releaseNotes = await _releaseRepository.FindReleaseNotes(resource.ReleaseNotesId);
                    }
                    else
                    {
                        // Create new Release Notes
                        releaseNotes = resource.ReleaseNotes;
                    }

                    // Map each ReleaseNote to this Release
                    foreach (var releaseNote in releaseNotes)
                    {
                        releaseReleaseNotes.Add(new ReleaseReleaseNote {Release = release, ReleaseNote = releaseNote});
                    }

                    // Add ReleaseReleaseNote to the Release
                    release.ReleaseReleaseNotes = releaseReleaseNotes;
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