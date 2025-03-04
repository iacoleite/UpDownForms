using UpDownForms.Models;
using UpDownForms.DTO.FormDTOs;

namespace UpDownForms.DTO.UserDTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public List<FormDTO> Forms { get; set; }
    }
}
