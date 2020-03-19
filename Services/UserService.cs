using System.Collections.Generic;
using System.Threading.Tasks;
using ReleaseNotes_WebAPI.Domain.Models.Auth;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Domain.Security;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Domain.Services.Communication;

namespace ReleaseNotes_WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<CreateUserResponse> CreateUserAsync(User user, params ERole[] userRoles)
        {
            var existingUser = await _userRepository.FindByEmailAsync(user.Email);
            if (existingUser != null)
                return new CreateUserResponse(false, "Email already in use", null);
            user.Password = _passwordHasher.HashPassword(user.Password);

            await _userRepository.AddAsync(user, userRoles);
            await _unitOfWork.CompleteAsync();

            return new CreateUserResponse(true, null, user);
        }

        public async Task<CreateUserResponse> ChangeUserPasswordAsync(User user, string newPassword)
        {
            var existingUser = await _userRepository.FindByEmailAsync(user.Email);
            if (existingUser == null)
            {
                return new CreateUserResponse(false, "Denne brukeren eksisterer ikke!", null);
            }

            if (_passwordHasher.PasswordMatches(newPassword, user.Password))
            {
                return new CreateUserResponse(false, "Brukeren bruker dette passorder nå", null);
            }

            existingUser.Password = _passwordHasher.HashPassword(user.Password);
            await _userRepository.ChangePassword(existingUser);
            await _unitOfWork.CompleteAsync();

            return new CreateUserResponse(true, "Brukeren har nå fått et nytt passord!", user);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userRepository.FindByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> ListAsync()
        {
            return await _userRepository.ListAsync();
        }
    }
}