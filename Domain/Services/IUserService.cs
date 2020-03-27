using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models.Auth;
using ReleaseNotes_WebAPI.Domain.Services.Communication;
using ReleaseNotes_WebAPI.Resources;
using ReleaseNotes_WebAPI.Resources.Auth;

namespace ReleaseNotes_WebAPI.Domain.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateUserAsync(User user, params ERole[] userRoles);

        Task<User> FindByEmailAsync(string email);

        Task<User> FindByIdAsync(int id);

        Task<IEnumerable<User>> ListAsync();

        Task<CreateUserResponse> ChangeUserPasswordAsync(User user,
            string password);

        Task<CreateUserResponse> UpdateUserAsync(User user, UpdateUserResource userResource);
    }
}