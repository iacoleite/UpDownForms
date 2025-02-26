using System.ComponentModel.DataAnnotations.Schema;

namespace UpDownForms.Models
{
    [Table("Answers")]
    public class Answer
    {
        public int Id { get; set; }
        public int ResponseId { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public int? OptionId { get; set; }
        public bool IsDeleted { get; set; }
        public Response Response { get; set; }
        public Question Question { get; set; }
        public Option Option { get; set; }
    }
}
