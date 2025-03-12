using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.OptionDTOs;

namespace UpDownForms.Models
{
    public class AnswerMultipleChoice : Answer
    {
        //public int OptionId { get; set; }
        public List<AnsweredOption> SelectedOptions { get; set; } = new List<AnsweredOption>();

        public AnswerMultipleChoice() { }

        public AnswerMultipleChoice(CreateAnswerMultipleChoiceDTO createAnswerDTO)
        {
            //this.ResponseId = createAnswerDTO.ResponseId;
            this.QuestionId = createAnswerDTO.QuestionId;
            this.IsDeleted = createAnswerDTO.IsDeleted;
            ////this.OptionId = createAnswerDTO.OptionId;
            foreach (var optionId in createAnswerDTO.SelectedOptions)
            {
                this.SelectedOptions.Add(new AnsweredOption { OptionId = optionId });
            }
            
        }
    }
}
