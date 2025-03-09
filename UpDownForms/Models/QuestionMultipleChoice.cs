using UpDownForms.DTO.OptionDTOs;
using UpDownForms.DTO.QuestionDTOs;

namespace UpDownForms.Models
{
    public class QuestionMultipleChoice : Question
    {
        public QuestionType Type { get; set; }
        public bool HasCorrectAnswer { get; set; }
        public List<Option> Options { get; set; } = new List<Option>();

        public QuestionMultipleChoice() { }

        public QuestionMultipleChoice(CreateQuestionMultipleChoiceDTO createQuestionDTO)
        {
            this.FormId = createQuestionDTO.FormId;
            this.Text = createQuestionDTO.Text;
            this.Order = createQuestionDTO.Order;
            this.Type = Enum.Parse<QuestionType>(createQuestionDTO.Type);
            this.IsRequired = createQuestionDTO.IsRequired;
            this.IsDeleted = false;
            this.HasCorrectAnswer = createQuestionDTO.HasCorrectAnswer;
        }
        public void AddOption(Option option)
        {
            this.Options.Add(option);
        }
        public void RemoveOption(Option option)
        {
            this.Options.Remove(option);
        }
        //public List<OptionDTO> GetOptionsDTOs()
        //{
        //    return this.Options.Select(o => o.ToOptionDTO()).ToList();
        //}
    }

}
