using UpDownForms.Models;

namespace UpDownForms.DTO.QuestionDTOs
{
    public class CreateQuestionDTO
    {
        public int FormId { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
    }
}