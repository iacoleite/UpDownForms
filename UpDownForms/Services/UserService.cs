using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.ApiResponse;
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

        public async Task<ApiResponse<IEnumerable<UserDetailsDTO>>> GetUsers()
        {
            //return await _context.Users
            //    .Where(u => !u.IsDeleted)                
            //    .Select(u => u.ToUserDetailsDTO()).ToListAsync();

            var response = await _context.Users.Where(u => !u.IsDeleted).ToListAsync();

            return new ApiResponse<IEnumerable<UserDetailsDTO>>(true, "ok!", response.Select(u => u.ToUserDetailsDTO()).ToList());
        }

        public async Task<ApiResponse<UserDetailsDTO>> GetUser(string id)
        {
            var response = await _context.Users.FindAsync(id);
            if (response == null)
            {
                return new ApiResponse<UserDetailsDTO>(false, "User not found", null);
            }
            return new ApiResponse<UserDetailsDTO>(true, "OK", response.ToUserDetailsDTO());
        }

        //public async Task<(IdentityResult IdentityResult, User CreatedUser)> PostUser([FromBody] CreateUserDTO createdUserDTO)
        public async Task<ApiResponse<UserDetailsDTO>> PostUser([FromBody] CreateUserDTO createdUserDTO)
        {
            if (createdUserDTO == null)
            {
                return new ApiResponse<UserDetailsDTO>(false, "Missing user data", null);
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

            if (!result.Succeeded)
            {
                return new ApiResponse<UserDetailsDTO>(false, "User creation failed", null);
            }

            return new ApiResponse<UserDetailsDTO>(true, "User created successfully", user.ToUserDetailsDTO());
        }

        public async Task<ApiResponse<UserDetailsDTO>> UpdateUser(string id, UpdateUserDTO updatedUserDTO)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return new ApiResponse<UserDetailsDTO>(false, "User not found", null);
            }

            if (!string.IsNullOrEmpty(updatedUserDTO.Name))
            {
                user.Name = updatedUserDTO.Name;
            }

            if (!string.IsNullOrEmpty(updatedUserDTO.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var updatePasswordResult = await _userManager.ResetPasswordAsync(user, token, updatedUserDTO.Password);
                if (!updatePasswordResult.Succeeded)
                {
                    return new ApiResponse<UserDetailsDTO>(false, "Password update failed", null);
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return new ApiResponse<UserDetailsDTO>(false, "User update failed", null);
            }

            return new ApiResponse<UserDetailsDTO>(true, "User updated successfully", user.ToUserDetailsDTO());
        }

        public async Task<ApiResponse<UserDetailsDTO>> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return new ApiResponse<UserDetailsDTO>(false, "User not found", null);
            }
            user.DeleteUser();
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return new ApiResponse<UserDetailsDTO>(true, "User deleted successfully", user.ToUserDetailsDTO());
        }
    }
}



