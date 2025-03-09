using UpDownForms.DTO.OptionDTOs;
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

    public class CreateQuestionMultipleChoiceDTO : CreateQuestionDTO
    {
        public List<CreateOptionDTO> Options { get; set; } = new List<CreateOptionDTO>();
        public bool HasCorrectAnswer { get; set; }
    }

    public class CreateQuestionOpenEndedDTO : CreateQuestionDTO
    {
        public string Answer { get; set; }
    }
}