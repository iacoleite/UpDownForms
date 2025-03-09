using System.ComponentModel.DataAnnotations.Schema;
using UpDownForms.DTO.OptionDTOs;

namespace UpDownForms.Models
{

    // This class is used to 
    [Table("Options")]
    public class Option
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public QuestionMultipleChoice QuestionMultipleChoice { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsCorrect { get; set; }

        public Option() { }

        public Option(CreateOptionDTO createOptionDTO)
        {
            //this.QuestionId = createOptionDTO.QuestionId;
            this.Text = createOptionDTO.Text;
            this.Order = createOptionDTO.Order;
        }

        public OptionDTO ToOptionDTO()
        {
            return new OptionDTO
            {
                Id = this.Id,
                QuestionId = this.QuestionId,
                Text = this.Text,
                Order = this.Order
            };
        }
    }
}
