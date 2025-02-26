using System.ComponentModel.DataAnnotations.Schema;

namespace UpDownForms.Models
{
    [Table("Forms")]
    public class Form
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }
        public User User { get; set; }
        public List<Question> Questions { get; set; }
        public List<Response> Responses { get; set; }
    }
}
