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
        public int Id { get; set; }
        public int FormId { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        //public string Type { get; set; }
        public bool IsRequired { get; set; }
        public bool IsDeleted { get; set; }
        public Form Form { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
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
