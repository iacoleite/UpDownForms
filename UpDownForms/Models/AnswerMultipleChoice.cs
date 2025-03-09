using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.OptionDTOs;

namespace UpDownForms.Models
{
    public class AnswerMultipleChoice : Answer
    {
        public int OptionId { get; set; }
        public Option Options { get; set; }

        public AnswerMultipleChoice() { }

        public AnswerMultipleChoice(CreateAnswerMultipleChoiceDTO createAnswerDTO) : base(createAnswerDTO)
        {
            //this.OptionId = createAnswerDTO.OptionId;
            this.Options = createAnswerDTO.Options;
        }
    }
}
