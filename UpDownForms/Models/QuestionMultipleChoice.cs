using System.ComponentModel.DataAnnotations;
using UpDownForms.DTO.OptionDTOs;
using UpDownForms.DTO.QuestionDTOs;

namespace UpDownForms.Models
{
    public class QuestionMultipleChoice : Question
    {
        [Required]
        public QuestionType QuestionMCType { get; set; }
        [Required]
        public bool HasCorrectAnswer { get; set; }
        [Required]
        public List<Option> Options { get; set; } = new List<Option>();
        
        public QuestionMultipleChoice() {
            //Type = "MultipleChoice";
        }


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
                QuestionType = this.QuestionMCType,
                Options = this.Options != null ? this.Options.Where(o => o.IsDeleted == false).Select(o => o.ToOptionDTO()).ToList() : null
            };
            return multipleChoiceDto;
        }


        public QuestionMultipleChoice(CreateQuestionMultipleChoiceDTO createQuestionDTO)
        {
            this.FormId = createQuestionDTO.FormId;
            this.Text = createQuestionDTO.Text;
            this.Order = createQuestionDTO.Order;
            this.QuestionMCType = Enum.Parse<QuestionType>(createQuestionDTO.Type);
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

        public override void DeleteQuestion()
        {
            this.IsDeleted = true;
            foreach (var o in Options)
            {
                o.DeleteOption();
            }
        }

        public void UndeleteQuestionAndOptions()
        {
            this.IsDeleted = false;
            foreach (var o in Options)
            {
                o.UndeleteOption();
            }
        }
        //public List<OptionDTO> GetOptionsDTOs()
        //{
        //    return this.Options.Select(o => o.ToOptionDTO()).ToList();
        //}
    }

}
