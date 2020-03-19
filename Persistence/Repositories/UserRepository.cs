using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReleaseNotes_WebAPI.Domain.Models.Auth;
using ReleaseNotes_WebAPI.Domain.Repositories;
using ReleaseNotes_WebAPI.Persistence.Contexts;

namespace ReleaseNotes_WebAPI.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user, ERole[] userRoles)
        {
            var roles = await _context.Roles.ToListAsync();

            foreach (var role in roles)
            {
                foreach (var userRole in userRoles)
                {
                    if (role.Name.Equals(userRole.ToString()))
                    {
                        user.UserRoles.Add(new UserRole {RoleId = role.Id});
                    }
                }
            }

            _context.Users.Add(user);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> ListAsync()
        {
            return await _context.Users.Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role).ToListAsync();
        }
    }
}