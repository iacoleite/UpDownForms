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
        public Question Question { get; set; }

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
