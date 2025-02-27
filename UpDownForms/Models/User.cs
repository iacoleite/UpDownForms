using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UpDownForms.DTO;

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
    }

    
    
}


    

