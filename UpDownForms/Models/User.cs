using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using UpDownForms.DTO.UserDTOs;
using UpDownForms.Security;

namespace UpDownForms.Models
{
    [Table("Users")]
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public List<Form> Forms { get; set; }

        public User()
        {
        }

        public User(CreateUserDTO dto)
        {
            this.Name = dto.Name;
            this.Email = dto.Email;
            this.CreatedAt = DateTime.UtcNow;
        }

        public UserDetailsDTO ToUserDetailsDTO()
        {
            return new UserDetailsDTO
            {
                Id = this.Id,
                Name = this.Name,
                Email = this.Email,
                PasswordHash = this.PasswordHash,
                CreatedAt = this.CreatedAt,
                IsDeleted = this.IsDeleted
            };
        }

        public void UpdateUser(UpdateUserDTO updatedUserDTO)
        {
            if (updatedUserDTO.Name != null)
            {
                this.Name = updatedUserDTO.Name;
            }

            if (updatedUserDTO.Password != null)
            {
                this.PasswordHash = updatedUserDTO.Password;
            }
            this.UpdatedAt = DateTime.UtcNow;
            UndeleteUser();
        }

        public void DeleteUser()
        {
            this.IsDeleted = true;
        }

        public void UndeleteUser()
        {
            this.IsDeleted = false;
        }

        private readonly IPasswordHelper _passwordHelper;
        public User(IPasswordHelper passwordHelper)
        {
            _passwordHelper = passwordHelper;
        }
    }
}


    

