using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Persistence.Contexts;
using ReleaseNotes_WebAPI.Services;

namespace ReleaseNotes_WebAPI.Persistence.Repositories
{
    public class AzureInformationRepository : BaseRepository, IAzureInformationRepository
    {
        public AzureInformationRepository(AppDbContext context) : base(context)
        {
        }

        public async void AddAsync(AzureInformation azureInformation)
        {
            await _context.AzureInformations.AddAsync(azureInformation);
        }
        public void Update(AzureInformation azureInformation)
        {
            _context.AzureInformations.Update(azureInformation);
        }

        public Task<AzureInformation> FindById(int id)
        {
            return _context.AzureInformations.SingleOrDefaultAsync(a => a.Id == id);
        }
    }
}