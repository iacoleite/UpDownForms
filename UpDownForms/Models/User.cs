using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UpDownForms.DTO.User;

namespace UpDownForms.Models
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public List<Form> Forms { get; set; }

        public User()
        {
        }

        public User(CreateUserDTO dto)
        {
            this.Name = dto.Name;
            this.Email = dto.Email;
            this.CreatedAt = DateTime.UtcNow;
            // PasswordHash = dto.PasswordHash ?? need to generate the Hash 
            this.PasswordHash = dto.Password;
        }

        public UserDetailsDTO ToUserDetailsDTO()
        {
            return new UserDetailsDTO
            {
                Id = this.Id,
                Name = this.Name,
                Email = this.Email,
                //PasswordHash = this.PasswordHash,
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
            this.CreatedAt = DateTime.UtcNow;
            this.IsDeleted = false;
        }

        public void DeleteUser()
        {
            this.IsDeleted = true;
        }

        public void UndeleteUser()
        {
            this.IsDeleted = false;
        }
    }

    
    
}


    

