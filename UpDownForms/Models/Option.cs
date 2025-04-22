using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UpDownForms.DTO.OptionDTOs;

namespace UpDownForms.Models
{

    [Table("Options")]
    public class Option : IVerifyOwnership
    {
        public int Id { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Text { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        public QuestionMultipleChoice QuestionMultipleChoice { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public bool IsCorrect { get; set; }
        [Required]
        public List<AnsweredOption> AnsweredOptions { get; set; } = new List<AnsweredOption>();

        public string UserId => ((IVerifyOwnership)QuestionMultipleChoice).UserId;

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

        public string GetUserId()
        {
            return UserId;
        }
    }
}
