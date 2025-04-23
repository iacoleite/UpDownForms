using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.FormDTOs;
using UpDownForms.DTO.QuestionDTOs;
using UpDownForms.DTO.UserDTOs;
using UpDownForms.Models;
using UpDownForms.Pagination;
using UpDownForms.Security;
using UpDownForms.Services.Interfaces;

namespace UpDownForms.Services
{
    public class UserService : IUserService
    {
        private readonly IUpDownFormsContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILoggedUserService _userService;

        public UserService(IUpDownFormsContext context, UserManager<User> userManager, ILoggedUserService userService)
        {
            _context = context;
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<Pageable<UserDetailsDTO>> GetUsers(PageParameters pageParameters)
        {
            var response = _context.Users.Where(u => !u.IsDeleted);
            if (response == null)
            {
                throw new EntityNotFoundException();
            }
            var orderParam = PageParamConfigurator.SetSortOrder<UserDetailsDTO>(pageParameters);


            var pageable = await Pageable<UserDetailsDTO>.ToPageable(response.OrderBy(orderParam).Select(u => u.ToUserDetailsDTO()), pageParameters);
            if (pageable.Items.Count() == 0)
            {
                throw new EntityNotFoundException();
            }
            return pageable;
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

            var result = await _userManager.CreateAsync(user, createdUserDTO.Password);
            if (!result.Succeeded)
            {
                throw new Exception("Can't create user");
            }

            return user.ToUserDetailsDTO();
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



