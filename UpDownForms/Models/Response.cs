using System.ComponentModel.DataAnnotations.Schema;

namespace UpDownForms.Models
{
    [Table("Responses")]
    public class Response
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public string RespondentEmail { get; set; }
        public DateTime SubmittedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Form Form { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
