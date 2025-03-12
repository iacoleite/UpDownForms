using UpDownForms.DTO.QuestionDTOs;
using UpDownForms.DTO.ResponseDTOs;
using UpDownForms.DTO.UserDTOs;
using UpDownForms.Models;

namespace UpDownForms.DTO.FormDTOs
{
    public class FormDetailsDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }
        //public UserDetailsDTO User { get; set; }
        //public List<QuestionDTO> Questions { get; set; }
        //public List<ResponseDTO> Responses { get; set; }
    }
}