using System.ComponentModel.DataAnnotations.Schema;
using UpDownForms.DTO.AnswersDTOs;

namespace UpDownForms.Models
{
    [Table("Answers")]
    public class Answer
    {
        public int Id { get; set; }
        public int ResponseId { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public int? OptionId { get; set; }
        public bool IsDeleted { get; set; }
        public Response Response { get; set; }
        public Question Question { get; set; } 
        public Option Option { get; set; }

        public Answer()
        {
        }

        public Answer(CreateAnswerDTO createAnswerDTO)
        {
            //this.ResponseId = createAnswerDTO.ResponseId;
            this.QuestionId = createAnswerDTO.QuestionId;
            this.AnswerText = createAnswerDTO.AnswerText;
            this.OptionId = createAnswerDTO.OptionId;
            //this.IsDeleted = false;
        }
        public AnswerDTO ToAnswerDTO()
        {
            return new AnswerDTO
            {
                Id = this.Id,
                ResponseId = this.ResponseId,
                QuestionId = this.QuestionId,
                AnswerText = this.AnswerText,
                OptionId = this.OptionId,
                IsDeleted = this.IsDeleted
            };
        }



        
    }
}
