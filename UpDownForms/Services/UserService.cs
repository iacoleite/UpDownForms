using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.UserDTOs;
using UpDownForms.Models;
using UpDownForms.Security;

namespace UpDownForms.Services
{
    public class UserService
    {
        private readonly UpDownFormsContext _context;
        private readonly IPasswordHelper _passwordHelper;
        private readonly UserManager<User> _userManager;

        public UserService(UpDownFormsContext context, IPasswordHelper passwordHelper, UserManager<User> userManager)
        {
            _context = context;
            _passwordHelper = passwordHelper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserDetailsDTO>> GetUsers()
        {
            //return await _context.Users
            //    .Where(u => !u.IsDeleted)                
            //    .Select(u => u.ToUserDetailsDTO()).ToListAsync();

            var users = await _context.Users.Where(u => !u.IsDeleted).ToListAsync();

            return users.Select(u => u.ToUserDetailsDTO()).ToList();
        }

        public async Task<UserDetailsDTO> GetUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return user.ToUserDetailsDTO();
        }

        public async Task<(IdentityResult IdentityResult, User CreatedUser)> PostUser([FromBody] CreateUserDTO createdUserDTO)
        {
            if (createdUserDTO == null)
            {
                throw new ArgumentNullException("Missing user data");
            }
            var user = new User
            {
                UserName = createdUserDTO.Email,
                Name = createdUserDTO.Name,
                Email = createdUserDTO.Email,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                Forms = new List<Form>()
            };
            var result = await _userManager.CreateAsync(user, createdUserDTO.Password);
            
            return (result, user);
        }
    }
}



