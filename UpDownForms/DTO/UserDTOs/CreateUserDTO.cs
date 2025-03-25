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
        [Required(AllowEmptyStrings = false)]

        public string Name { get; set; }
        [Required(AllowEmptyStrings = false)]
        [EmailAddress]

        public string Email { get; set; }
        //public string PasswordHash { get; set; }
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[\\-._@+])[a-zA-Z0-9\\-._@+]+$")]
        public string Password { get; set; }

        //public DateTime CreatedAt { get; set; }
        //public bool IsDeleted { get; set; }
        //public List<Form> Forms { get; set; }
    }


}
