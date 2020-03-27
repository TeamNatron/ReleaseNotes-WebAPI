using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Domain.Repositories
{
    public interface IAzureInformationRepository
    {
        
        void AddAsync(AzureInformation product);

        void Update(AzureInformation product);

        Task<AzureInformation> FindById(int id);
    }
}