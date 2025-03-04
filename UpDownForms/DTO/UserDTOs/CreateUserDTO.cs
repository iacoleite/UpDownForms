using Microsoft.AspNetCore.Identity;
using UpDownForms.Models;
using UpDownForms.Security;

namespace UpDownForms.DTO.UserDTOs
{
    public class CreateUserDTO
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        //public string PasswordHash { get; set; }
        public string Password { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public bool IsDeleted { get; set; }
        //public List<Form> Forms { get; set; }
    }

    public static class UserDtoExt
    {
        public static Models.User ToUserDto(this CreateUserDTO userDto)
        {
            var user = new Models.User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                Forms = new List<Form>()
            };
            return user;
        }
    }
}
