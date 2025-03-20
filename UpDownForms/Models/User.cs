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
        private string _name;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        private bool _isDeleted;
        private List<Form> _forms;
        private IPasswordHelper _passwordHelper;

        [Required]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [Required]
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
        }

        [Required]
        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set => _updatedAt = value;
        }

        [Required]
        public bool IsDeleted
        {
            get => _isDeleted;
            set => _isDeleted = value;
        }

        [Required]
        public List<Form> Forms
        {
            get => _forms;
            set => _forms = value;
        }

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

        //private readonly IPasswordHelper _passwordHelper;
        public User(IPasswordHelper passwordHelper)
        {
            _passwordHelper = passwordHelper;
        }
    }
}


    

