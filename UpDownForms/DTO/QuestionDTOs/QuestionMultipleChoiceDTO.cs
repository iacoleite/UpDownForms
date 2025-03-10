using UpDownForms.DTO.OptionDTOs;

namespace UpDownForms.DTO.QuestionDTOs
{
    public class QuestionMultipleChoiceDTO : QuestionDTO
    {
        public bool HasCorrectAnswer { get; set; }
        public List<OptionDTO> Options { get; set; }
    }
}
