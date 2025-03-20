using System.ComponentModel.DataAnnotations.Schema;
using UpDownForms.DTO.OptionDTOs;

namespace UpDownForms.Models
{

    // This class is used to 
    [Table("Options")]
    public class Option
    {
        private int _id;
        private int _questionId;
        private string _text;
        private int _order;
        private QuestionMultipleChoice _questionMultipleChoice;
        private bool _isDeleted;
        private bool _isCorrect;
        private List<AnsweredOption> _answeredOptions = new List<AnsweredOption>();

        public int Id { 
            get => _id; 
            set => _id = value; 
        }
        public int QuestionId { 
            get => _questionId;
            set => _questionId = value; 
        }
        public string Text { 
            get => _text; 
            set => _text = value; 
        }
        public int Order { 
            get => _order; 
            set => _order = value; 
        }
        public QuestionMultipleChoice QuestionMultipleChoice { 
            get => _questionMultipleChoice; 
            set => _questionMultipleChoice = value; 
        }
        public bool IsDeleted { 
            get => _isDeleted; 
            set => _isDeleted = value; 
        }
        public bool IsCorrect {
            get => _isCorrect ; 
            set => _isCorrect = value; 
        }
        public List<AnsweredOption> AnsweredOptions { 
            get => _answeredOptions;  
            set => _answeredOptions = value; 
        } 

        public Option() { }

        public Option(CreateOptionDTO createOptionDTO)
        {
            //this.QuestionId = createOptionDTO.QuestionId;
            this.Text = createOptionDTO.Text;
            this.Order = createOptionDTO.Order;
            this.IsCorrect = createOptionDTO.IsCorrect;
        }

        public OptionDTO ToOptionDTO()
        {
            return new OptionDTO
            {
                Id = this.Id,
                QuestionId = this.QuestionId,
                Text = this.Text,
                Order = this.Order,
                IsDeleted = this.IsDeleted,
                IsCorrect = this.IsCorrect
                
            };
        }

        public void DeleteOption()
        {
            this.IsDeleted = true;
        }

        public void UndeleteOption()
        {
            this.IsDeleted = false;
        }
    }
}
