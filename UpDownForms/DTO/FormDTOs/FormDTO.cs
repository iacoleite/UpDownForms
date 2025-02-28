using UpDownForms.DTO.User;
using UpDownForms.Models;

namespace UpDownForms.DTO.FormDTOs
{
    public class FormDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }
        public UserDetailsDTO User { get; set; }
        public List<Question> Questions { get; set; }
        public List<Response> Responses { get; set; }
    }
}