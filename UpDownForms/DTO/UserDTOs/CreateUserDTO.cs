using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using UpDownForms.Models;
using UpDownForms.Security;

namespace UpDownForms.DTO.UserDTOs
{
    public class CreateUserDTO
    {
        //public int Id { get; set; }
        //[Required]
        public string Name { get; set; }
        public string Email { get; set; }
        //public string PasswordHash { get; set; }=
        public string Password { get; set; }

        //public DateTime CreatedAt { get; set; }
        //public bool IsDeleted { get; set; }
        //public List<Form> Forms { get; set; }
    }


}
