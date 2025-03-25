using System.ComponentModel.DataAnnotations;
using UpDownForms.Models;

namespace UpDownForms.DTO.AnswersDTOs
{
    public class AnswerOpenEndedResponseDTO : AnswerDTO
    {
        //public int Id { get; set; }
        //public int ResponseId { get; set; }
        //public int QuestionId { get; set; }
        [Required(AllowEmptyStrings = false)]

        public string AnswerText { get; set; }
        //public int? OptionId { get; set; }
        //public bool IsDeleted { get; set; }

    }
}
