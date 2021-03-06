using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ReleaseNotes_WebAPI.Domain.Models;
using ReleaseNotes_WebAPI.Domain.Models.Auth;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Domain.Security;
using ReleaseNotes_WebAPI.Domain.Services;
using ReleaseNotes_WebAPI.Domain.Services.Communication;
using ReleaseNotes_WebAPI.Resources;

namespace ReleaseNotes_WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAzureInformationRepository _azureInformationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher,
            IAzureInformationRepository azureInformationRepository, IMapper mapper)
        {
            _azureInformationRepository = azureInformationRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
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
            if (user == null)
            {
                return new CreateUserResponse(false, "Denne brukeren eksisterer ikke!", null);
            }

            if (_passwordHasher.PasswordMatches(newPassword, user.Password))
            {
                return new CreateUserResponse(false, "Passordet kan ikke være likt det gamle!", null);
            }

            try
            {
                user.Password = _passwordHasher.HashPassword(newPassword);
                _userRepository.Update(user);
                await _unitOfWork.CompleteAsync();
                return new CreateUserResponse(true, "Brukeren har nå fått et nytt passord!", user);
            }
            catch (Exception e)
            {
                return new CreateUserResponse(false, $"Det oppsto en feil: {e.Message}", null);
            }
        }

        public async Task<CreateUserResponse> UpdateUserAsync(User user, UpdateUserResource userResource)
        {
            try
            {
                if (user == null)
                {
                    return new CreateUserResponse(false, "Denne brukeren eksisterer ikke!", null);
                }

                if (userResource.AzureInformation != null)
                {
                    if (user.AzureInformation == null)
                    {
                        var newAzureInfo = _mapper.Map<AzureInformation>(userResource.AzureInformation);
                        _azureInformationRepository.AddAsync(newAzureInfo);
                        user.AzureInformation = newAzureInfo;
                    }
                    else if (user.AzureInformationId != null)
                    {
                        var aid = (int) user.AzureInformationId;
                        var existingAzureInfo = await _azureInformationRepository.FindById(aid);
                        _mapper.Map(userResource.AzureInformation, existingAzureInfo);
                    }

                    await _unitOfWork.CompleteAsync();
                    return new CreateUserResponse(true, "Bruker oppdatert.", user);
                }

                return new CreateUserResponse(false, "Bruker ikke oppdatert.", null);
            }
            catch (Exception e)
            {
                return new CreateUserResponse(false, "En feil oppstod: " + e.Message, null);
            }
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userRepository.FindByEmailAsync(email);
        }

        public async Task<User> FindByIdAsync(int id)
        {
            return await _userRepository.FindByIdAsync(id);
        }

        public async Task<IEnumerable<User>> ListAsync()
        {
            return await _userRepository.ListAsync();
        }
    }
}