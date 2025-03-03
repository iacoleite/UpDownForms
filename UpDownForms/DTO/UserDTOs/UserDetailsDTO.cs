using UpDownForms.Models;

namespace UpDownForms.DTO.User
{
    public class UserDetailsDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        //public string PasswordHash { get; set; }
        //public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        //public List<Form> Forms { get; set; }
    }
}
