using System.ComponentModel.DataAnnotations;

namespace UpDownForms.DTO.UserDTOs
{
    public class LoginUserDTO
    {
        [Required(AllowEmptyStrings = false)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false)]

        public string Password { get; set; }
    }
}
