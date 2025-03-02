using System.ComponentModel.DataAnnotations.Schema;
using UpDownForms.DTO.QuestionDTOs;

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


        public Question()
        {
        }

        public Question(CreateQuestionDTO createQuestionDTO)
        {
            this.Text = createQuestionDTO.Text;
            this.Order = createQuestionDTO.Order;
            this.Type = createQuestionDTO.Type;
            this.IsRequired = createQuestionDTO.IsRequired;
            this.IsDeleted = false;
        }

        public QuestionDTO ToQuestionDTO()
        {
            return new QuestionDTO
            {
                Id = this.Id,
                FormId = this.FormId,
                Text = this.Text,
                Order = this.Order,
                Type = this.Type,
                IsRequired = this.IsRequired,
                IsDeleted = this.IsDeleted
            };
        }

        public QuestionDTO ToQuestionDetailsDTO()
        {
            return new QuestionDTO
            {
                Id = this.Id,
                FormId = this.FormId,
                Text = this.Text,
                Order = this.Order,
                Type = this.Type,
                IsRequired = this.IsRequired,
                IsDeleted = this.IsDeleted
            };
        }

        public QuestionDTO ToQuestionDetailsWithOptionsDTO()
        {
            return new QuestionDTO
            {
                //Id = this.Id,
                //FormId = this.FormId,
                Text = this.Text,
                Order = this.Order,
                Type = this.Type,
                IsRequired = this.IsRequired,
                IsDeleted = this.IsDeleted,
                Options = this.Options.Select(o => o.ToOptionDTO()).ToList()
            };
        }

        public QuestionDTO ToQuestionDetailsWithOptionAnswersDTO()
        {
            return new QuestionDTO
            {
                Id = this.Id,
                FormId = this.FormId,
                Text = this.Text,
                Order = this.Order,
                Type = this.Type,
                IsRequired = this.IsRequired,
                IsDeleted = this.IsDeleted,
                Options = this.Options.Select(o => o.ToOptionDTO()).ToList(),
                Answers = this.Answers.Select(a => a.ToAnswerDTO()).ToList()
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

        public void DeleteQuestion()
        {
            this.IsDeleted = true;
        }

        public void AddOption(Option option)
        {
            this.Options.Add(option);
        }
        public void RemoveOption(Option option)
        {
            this.Options.Remove(option);
        }


    }
}
