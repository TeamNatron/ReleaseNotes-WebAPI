using System.Threading.Tasks;

namespace ReleaseNotes_WebAPI.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}