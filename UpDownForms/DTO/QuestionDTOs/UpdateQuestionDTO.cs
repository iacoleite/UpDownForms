using UpDownForms.Models;

namespace UpDownForms.DTO.QuestionDTOs
{
    public class UpdateQuestionDTO
    {
        public string Text { get; set; }
        public int Order { get; set; }
        //public QuestionType Type { get; set; }
        public bool IsRequired { get; set; }
    }
}