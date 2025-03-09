using System.Security.Cryptography.X509Certificates;
using UpDownForms.DTO.OptionDTOs;
using UpDownForms.Models;

namespace UpDownForms.DTO.AnswersDTOs
{
    public class CreateAnswerDTO
    {
        //public int Id { get; set; }
        public int ResponseId { get; set; }
        public int QuestionId { get; set; }
        //public string? AnswerText { get; set; }
        //
        public bool IsDeleted { get; set; }
        
    }

    public class CreateAnswerMultipleChoiceDTO() : CreateAnswerDTO
    {
        //public int OptionId { get; set; }
        public Option Options { get; set; }
    }

    public class CreateAnswerOpenEndedDTO() : CreateAnswerDTO
    {
        public string AnswerText { get; set; }
    }
}
