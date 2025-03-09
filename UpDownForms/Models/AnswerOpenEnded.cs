using UpDownForms.DTO.AnswersDTOs;

namespace UpDownForms.Models
{
    public class AnswerOpenEnded : Answer
    {
        public string AnswerText { get; set; }

        public AnswerOpenEnded()
        {
        }   

        public AnswerOpenEnded(CreateAnswerOpenEndedDTO createAnswerOpenEndedDTO) : base(createAnswerOpenEndedDTO)
        {
            this.AnswerText = createAnswerOpenEndedDTO.AnswerText;
        }
    }
}
