using UpDownForms.Models;

namespace UpDownForms.DTO.AnswersDTOs
{
    public class AnswerDTO
    {
        public int Id { get; set; }
        public int ResponseId { get; set; }
        public int QuestionId { get; set; }
        public string? AnswerText { get; set; }
        public int? OptionId { get; set; }
        public bool IsDeleted { get; set; }
        public AnswerDTO() { }

        public AnswerDTO(Answer answer)
        {
            this.Id = answer.Id;
            this.QuestionId = answer.QuestionId;
            this.AnswerText = answer.AnswerText;
            this.OptionId = answer.OptionId;
            this.IsDeleted = answer.IsDeleted;
        }

    }
}
