using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.OptionDTOs;

namespace UpDownForms.Models
{
    public class AnswerMultipleChoice : Answer
    {
        //public int OptionId { get; set; }
        public List<AnsweredOption> SelectedOptions { get; set; } = new List<AnsweredOption>();

        public AnswerMultipleChoice() { }

        public AnswerMultipleChoice(CreateAnswerMultipleChoiceDTO createAnswerDTO) : base(createAnswerDTO)
        {
            ////this.OptionId = createAnswerDTO.OptionId;
            //this.SelectedOptions = createAnswerDTO.OptionsId;
        }
    }
}
