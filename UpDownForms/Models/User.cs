using System.ComponentModel.DataAnnotations.Schema;

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
    }
}
