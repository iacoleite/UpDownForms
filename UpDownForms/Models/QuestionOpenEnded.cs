using System.ComponentModel.DataAnnotations;
using UpDownForms.DTO.QuestionDTOs;

namespace UpDownForms.Models
{
    public class QuestionOpenEnded : Question
    {
        public QuestionOpenEnded()
        {
            //Type = "OpenEnded";
        }
        public QuestionOpenEnded(CreateQuestionDTO createQuestionDTO) : base(createQuestionDTO)
        {
        }

        //public bool IsDeleted { get; set; }

        public QuestionOpenEnded(QuestionOpenEndedDTO questionOpenEndedDTO)
        {
            this.FormId = questionOpenEndedDTO.FormId;
            this.Text = questionOpenEndedDTO.Text;
            this.Order = questionOpenEndedDTO.Order;
            this.IsRequired = questionOpenEndedDTO.IsRequired;
            this.IsDeleted = false;
        }
        public QuestionOpenEnded(CreateQuestionOpenEndedDTO createQuestionOpenEndedDTO) 
        {
            this.FormId = createQuestionOpenEndedDTO.FormId;
            this.Text = createQuestionOpenEndedDTO.Text;
            
            this.Order = createQuestionOpenEndedDTO.Order;
            this.IsRequired = createQuestionOpenEndedDTO.IsRequired;
            this.IsDeleted = false; 
        }

        public void UpdateQuestionOpenEnded(UpdateQuestionOpenEndedDTO updateQuestionOpenEndedDTO)
        {
            this.Text = updateQuestionOpenEndedDTO.Text;
            this.Order = updateQuestionOpenEndedDTO.Order;
            this.IsRequired = updateQuestionOpenEndedDTO.IsRequired;
            this.IsDeleted = false;
        }
    }
}
