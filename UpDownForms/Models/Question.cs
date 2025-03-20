using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.OptionDTOs;
using UpDownForms.DTO.QuestionDTOs;

namespace UpDownForms.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")] // it's lowercase because Json
    [JsonDerivedType(typeof(QuestionMultipleChoice), typeDiscriminator: "MultipleChoice")]
    [JsonDerivedType(typeof(QuestionOpenEnded), typeDiscriminator: "OpenEnded")]
    [Table("Questions")]
    public abstract class Question
    {
        private int _id;
        private int _formId;
        private string _text;
        private int _order;
        private bool _isRequired;
        private bool _isDeleted;
        private Form _form;
        private List<Answer> _answers = new List<Answer>();

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public int FormId
        {
            get => _formId;
            set => _formId = value;
        }

        public string Text
        {
            get => _text;
            set => _text = value;
        }

        public int Order
        {
            get => _order;
            set => _order = value;
        }

        public bool IsRequired
        {
            get => _isRequired;
            set => _isRequired = value;
        }

        public bool IsDeleted
        {
            get => _isDeleted;
            set => _isDeleted = value;
        }

        public Form Form
        {
            get => _form;
            set => _form = value;
        }

        public List<Answer> Answers
        {
            get => _answers;
            set => _answers = value;
        }
        //public string Type { get; set; }


        public Question()
        {
            
        }

        public Question(CreateQuestionDTO createQuestionDTO)
        {
            this.FormId = createQuestionDTO.FormId;
            this.Text = createQuestionDTO.Text;
            this.Order = createQuestionDTO.Order;
            //this.Type = Enum.Parse<QuestionType>(createQuestionDTO.Type);
            this.IsRequired = createQuestionDTO.IsRequired;
            this.IsDeleted = false;
        }

        public virtual QuestionDTO ToQuestionDTO()
        {
            return new BaseQuestionDTO
            {
                Id = this.Id,
                FormId = this.FormId,
                Text = this.Text,
                Order = this.Order,
                Type = this.GetType().Name,
                IsRequired = this.IsRequired,
                IsDeleted = this.IsDeleted,
                //Options = this.Options.Select(o => o.ToOptionDTO()).ToList(),
                Answers = this.Answers.Select(a => a.ToAnswerDTO()).ToList()
            };
        }

        public QuestionDetailsDTO ToQuestionDetailsDTO()
        {
            return new QuestionDetailsDTO
            {
                Id = this.Id,
                FormId = this.FormId,
                Text = this.Text,
                Order = this.Order,
                Type = this.GetType().Name,
                IsRequired = this.IsRequired,
                IsDeleted = this.IsDeleted
            };
        }

        public void UpdateQuestion(UpdateQuestionDTO updateQuestionDTO)
        {
            if (updateQuestionDTO.Text != null)
            {
                this.Text = updateQuestionDTO.Text;
            }
            if (updateQuestionDTO.Order != null)
            {
                this.Order = (int)updateQuestionDTO.Order;
            }
            //if (updateQuestionDTO.Type != null)
            //{
            //    this.Type = (QuestionType)updateQuestionDTO.Type;
            //}
            if (updateQuestionDTO.IsRequired != null)
            {
                this.IsRequired = (bool)updateQuestionDTO.IsRequired;
            }
            this.IsDeleted = false;
        }

        public virtual void DeleteQuestion()
        {
            this.IsDeleted = true;
        }

      
        public List<AnswerDTO> GetAnswerDTOs()
        {
            return this.Answers.Select(a => a.ToAnswerDTO()).ToList();
        }


    }
}
