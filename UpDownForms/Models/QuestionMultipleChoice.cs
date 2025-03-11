using UpDownForms.DTO.OptionDTOs;
using UpDownForms.DTO.QuestionDTOs;

namespace UpDownForms.Models
{
    public class QuestionMultipleChoice : Question
    {
        public QuestionType QuestionType { get; set; }
        public bool HasCorrectAnswer { get; set; }
        public List<Option> Options { get; set; } = new List<Option>();

        public QuestionMultipleChoice() { }


        public override QuestionDTO ToQuestionDTO()
        {
            var baseDto = base.ToQuestionDTO(); 
            var multipleChoiceDto = new QuestionMultipleChoiceDTO()
            {
                Id = baseDto.Id,
                FormId = baseDto.FormId,
                Text = baseDto.Text,
                Order = baseDto.Order,
                Type = baseDto.Type,
                IsRequired = baseDto.IsRequired,
                IsDeleted = baseDto.IsDeleted,
                Answers = baseDto.Answers, 
                HasCorrectAnswer = this.HasCorrectAnswer,
                QuestionType = this.QuestionType,
                Options = this.Options.Select(o => o.ToOptionDTO()).ToList()
            };
            return multipleChoiceDto;
        }


        public QuestionMultipleChoice(CreateQuestionMultipleChoiceDTO createQuestionDTO)
        {
            this.FormId = createQuestionDTO.FormId;
            this.Text = createQuestionDTO.Text;
            this.Order = createQuestionDTO.Order;
            this.QuestionType = Enum.Parse<QuestionType>(createQuestionDTO.Type);
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
