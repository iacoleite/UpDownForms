using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.FormDTOs;

namespace UpDownForms.DTO.ResponseDTOs
{
    public class CreateResponseDTO
    {
        //public int Id { get; set; }
        public int FormId { get; set; }
        public string RespondentEmail { get; set; }
        //public DateTime SubmittedAt { get; set; }
        //public bool IsDeleted { get; set; }
        //public FormDTO Form { get; set; }
        //public List<AnswerDTO> Answers { get; set; }
    }
}
