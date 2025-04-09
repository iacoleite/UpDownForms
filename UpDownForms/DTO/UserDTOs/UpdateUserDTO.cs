using System.ComponentModel.DataAnnotations;

namespace UpDownForms.DTO.UserDTOs
{
    public class UpdateUserDTO
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[\\-._@+])[a-zA-Z0-9\\-._@+]+$")]
        public string Password { get; set; }
    }
}
