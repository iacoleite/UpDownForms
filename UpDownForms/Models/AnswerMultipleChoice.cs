using Microsoft.CodeAnalysis.CSharp.Syntax;
using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.OptionDTOs;

namespace UpDownForms.Models
{
    public class AnswerMultipleChoice : Answer
    {
        public List<AnsweredOption> SelectedOptions { get; set; } = new List<AnsweredOption>();
        public AnswerMultipleChoice() {
        }

        public AnswerMultipleChoice(CreateAnswerMultipleChoiceDTO createAnswerDTO)
        {
            //this.ResponseId = createAnswerDTO.ResponseId;
            this.QuestionId = createAnswerDTO.QuestionId;
            this.IsDeleted = false;
            ////this.OptionId = createAnswerDTO.OptionId;
            foreach (var optionId in createAnswerDTO.SelectedOptions)
            {
                this.SelectedOptions.Add(new AnsweredOption { OptionId = optionId });
            }
        }

        public AnswerMultipleChoiceResponseDTO ToAnswerMultipleChoiceResponseDTO()
        {
            return new AnswerMultipleChoiceResponseDTO
            {
                Id = this.Id,
                ResponseId = this.ResponseId,
                QuestionId = this.QuestionId,
                IsDeleted = this.IsDeleted
            };
        }

        public AnswerMultipleChoiceResponseDTO ToAnswerDTO()
        {
            var teste = new AnswerMultipleChoiceResponseDTO
            {

                Id = this.Id,
                ResponseId = this.ResponseId,
                QuestionId = this.QuestionId,
                //AnswerText = this.AnswerText,
                //OptionId = this.OptionId,
                IsDeleted = this.IsDeleted
            };
            return teste;
        }
    }
}
