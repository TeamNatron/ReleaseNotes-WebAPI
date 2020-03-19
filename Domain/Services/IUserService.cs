using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models.Auth;
using ReleaseNotes_WebAPI.Domain.Services.Communication;

namespace ReleaseNotes_WebAPI.Domain.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateUserAsync(User user, params ERole[] userRoles);

        Task<User> FindByEmailAsync(string email);

        Task<IEnumerable<User>> ListAsync();

        Task<CreateUserResponse> ChangeUserPasswordAsync(User user, string newPassword);
    }
}