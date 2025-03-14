using UpDownForms.Models;

namespace UpDownForms.DTO.AnswersDTOs
{
    public class AnswerMultipleChoiceResponseDTO : AnswerDTO
    {
        //public int Id { get; set; }
        //public int ResponseId { get; set; }
        //public int QuestionId { get; set; }
        //public string? AnswerText { get; set; }
        public List<int> SelectedOptions { get; set; } = new List<int>();
        //public bool IsDeleted { get; set; }

        public AnswerMultipleChoiceResponseDTO()
        {

        }

        public AnswerMultipleChoiceResponseDTO(AnswerMultipleChoice answer)
        {
            this.Id = answer.Id;
            this.ResponseId = answer.ResponseId;
            this.QuestionId = answer.QuestionId;
            //this.AnswerText = answer.AnswerText;
            this.IsDeleted = answer.IsDeleted;
            foreach (var option in answer.SelectedOptions)
            {
                this.SelectedOptions.Add(option.OptionId);
            }
        }
    }

    
}
