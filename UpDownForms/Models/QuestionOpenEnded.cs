using UpDownForms.DTO.QuestionDTOs;

namespace UpDownForms.Models
{
    public class QuestionOpenEnded : Question
    {
        public QuestionOpenEnded() : base()
        {
        }
        public QuestionOpenEnded(CreateQuestionDTO createQuestionDTO) : base(createQuestionDTO)
        {
        }

        public bool IsDeleted { get; set; }
    }
}
