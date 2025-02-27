using System.ComponentModel.DataAnnotations.Schema;

namespace UpDownForms.Models
{

    [Table("Questions")]
    public class Question
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public QuestionType Type { get; set; }
        public bool IsRequired { get; set; }
        public bool IsDeleted { get; set; }
        public Form Form { get; set; }
        public List<Option> Options { get; set; }
        public List<Answer> Answers { get; set; }  

    }
}
