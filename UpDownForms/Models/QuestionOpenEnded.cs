using UpDownForms.DTO.QuestionDTOs;

namespace UpDownForms.Models
{
    public class QuestionOpenEnded : Question
    {

        public bool IsDeleted { get; set; }

        public override QuestionDTO ToQuestionDTO()
        {
            var baseDto = base.ToQuestionDTO(); 
            var openEndedDto = new QuestionOpenEndedDTO()
            {
                Id = baseDto.Id,
                FormId = baseDto.FormId,
                Text = baseDto.Text,
                Order = baseDto.Order,
                Type = baseDto.Type, 
                IsRequired = baseDto.IsRequired,
                IsDeleted = baseDto.IsDeleted,
                Answers = baseDto.Answers 
                                          
            };
            return openEndedDto;
        }
    }
}
