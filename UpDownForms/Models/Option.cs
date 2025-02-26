using System.ComponentModel.DataAnnotations.Schema;

namespace UpDownForms.Models
{
    [Table("Options")]
    public class Option
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public Question Question { get; set; }
    }
}
