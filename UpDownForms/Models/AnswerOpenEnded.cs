using UpDownForms.DTO.AnswersDTOs;

namespace UpDownForms.Models
{
    public class AnswerOpenEnded : Answer
    {
        private string _answerText;

        public string AnswerText
        {
            get => _answerText;
            set => _answerText = value;
        }
        public AnswerOpenEnded()
        {
            //Type = "OpenEnded";
        }   

        public AnswerOpenEnded(CreateAnswerOpenEndedDTO createAnswerOpenEndedDTO) : base(createAnswerOpenEndedDTO)
        {
            this.AnswerText = createAnswerOpenEndedDTO.AnswerText;
        }

        public AnswerOpenEndedResponseDTO ToAnswerOpenEndedResponseDTO()
        {
            return new AnswerOpenEndedResponseDTO
            {
                Id = this.Id,
                ResponseId = this.ResponseId,
                QuestionId = this.QuestionId,
                AnswerText = this.AnswerText,
                IsDeleted = this.IsDeleted
            };
        }

        public AnswerOpenEndedResponseDTO ToAnswerDTO()
        {
            return new AnswerOpenEndedResponseDTO
            {
                Id = this.Id,
                ResponseId = this.ResponseId,
                QuestionId = this.QuestionId,
                AnswerText = this.AnswerText,
                //OptionId = this.OptionId,
                IsDeleted = this.IsDeleted
            };
        }
    }
}
