namespace UpDownForms.Models
{
    public class AnsweredOption
    {
        public int AnswerMultipleChoiceId { get; set; }
        public int OptionId { get; set; }
        public AnswerMultipleChoice AnswerMultipleChoice { get; set; }
        public Option Option { get; set; }
        

    }
}
