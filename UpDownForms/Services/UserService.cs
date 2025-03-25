using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UpDownForms.DTO.ApiResponse;
using UpDownForms.DTO.FormDTOs;
using UpDownForms.DTO.QuestionDTOs;
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
        private readonly IUserService _userService;

        public UserService(UpDownFormsContext context, IPasswordHelper passwordHelper, UserManager<User> userManager, IUserService userService)
        {
            _context = context;
            _passwordHelper = passwordHelper;
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<IEnumerable<UserDetailsDTO>> GetUsers()
        {
            var response = await _context.Users.Where(u => !u.IsDeleted).ToListAsync();
            if (response == null)
            {
                throw new EntityNotFoundException();
            }
            return response.Select(u => u.ToUserDetailsDTO()).ToList();
        }

        public async Task<UserDetailsDTO> GetUser(string id)
        {
            var response = await _context.Users.FindAsync(id);
            if (response == null)
            {
                throw new EntityNotFoundException();
            }
            var userId = _userService.GetLoggedInUserId();

            if (!response.Id.Equals(userId))
            {
                //return new ApiResponse<UserDetailsDTO>(false, "Logged user does not authorization to post to form", null);
                throw new UnauthorizedException("User not authorized to get data from another user");

            }

            return response.ToUserDetailsDTO();
        }

        public async Task<UserDetailsDTO> PostUser([FromBody] CreateUserDTO createdUserDTO)
        {
            if (createdUserDTO == null)
            {
                throw new BadHttpRequestException("Missing input data");
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
            //try
            //{
            var result = await _userManager.CreateAsync(user, createdUserDTO.Password);
            if (!result.Succeeded)
            {
                throw new Exception("Can't create user");
            }

            return user.ToUserDetailsDTO();
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}


        }

        public async Task<UserDetailsDTO> UpdateUser(string id, UpdateUserDTO updatedUserDTO)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }

            var loggedUserId = _userService.GetLoggedInUserId();
            if ((user.Id != loggedUserId))
            {
                throw new UnauthorizedException("User not authorized to update another user");
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
                    throw new Exception("Failed to update password");
                }
            }
            user.IsDeleted = false;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                throw new Exception("User update failed");
            }
            return user.ToUserDetailsDTO();
        }

        public async Task<UserDetailsDTO> DeleteUser(string id)
        {
            var loggedUserId = _userService.GetLoggedInUserId();

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }

            if (user.Id != loggedUserId)
            {
                throw new UnauthorizedException("User not authorized to update another user");
            }
            user.DeleteUser();
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user.ToUserDetailsDTO();
        }
    }
}



