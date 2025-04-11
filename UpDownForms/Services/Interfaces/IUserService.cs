using Microsoft.AspNetCore.Mvc;
using UpDownForms.DTO.UserDTOs;
using UpDownForms.Pagination;

namespace UpDownForms.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDetailsDTO> DeleteUser(string id);
        Task<UserDetailsDTO> GetUser(string id);
        Task<Pageable<UserDetailsDTO>> GetUsers(PageParameters pageParameters);
        Task<UserDetailsDTO> PostUser([FromBody] CreateUserDTO createdUserDTO);
        Task<UserDetailsDTO> UpdateUser(string id, UpdateUserDTO updatedUserDTO);
    }
}