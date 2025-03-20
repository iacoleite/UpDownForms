namespace UpDownForms.Models
{
    public class AnsweredOption
    {
        private int _answerMultipleChoiceId;
        private int _optionId;
        private AnswerMultipleChoice _answerMultipleChoice;
        private Option _option;

        public int AnswerMultipleChoiceId
        {
            get => _answerMultipleChoiceId;
            set => _answerMultipleChoiceId = value;
        }

        public int OptionId
        {
            get => _optionId;
            set => _optionId = value;
        }

        public AnswerMultipleChoice AnswerMultipleChoice
        {
            get => _answerMultipleChoice;
            set => _answerMultipleChoice = value;
        }

        public Option Option
        {
            get => _option;
            set => _option = value;
        }

    }
}
