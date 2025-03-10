using System.ComponentModel.DataAnnotations.Schema;
using UpDownForms.DTO.ResponseDTOs;

namespace UpDownForms.Models
{
    [Table("Responses")]
    public class Response
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public string? RespondentEmail { get; set; }
        public DateTime SubmittedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Form Form { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();

        public Response()
        {
        }

        public Response(CreateResponseDTO createResponseDTO)
        {
            this.FormId = createResponseDTO.FormId;
            this.RespondentEmail = createResponseDTO.RespondentEmail;
            this.SubmittedAt = DateTime.UtcNow;
        }

        public ResponseDTO ToResponseDTO()
        {
            return new ResponseDTO
            {
                Id = this.Id,
                FormId = this.FormId,
                RespondentEmail = this.RespondentEmail,
                SubmittedAt = this.SubmittedAt,
                IsDeleted = this.IsDeleted,
                Form = this.Form != null ? this.Form.ToFormDTO() : null,
                Answers = this.Answers != null ? this.Answers.Select(a => a.ToAnswerDTO()).ToList() : null
            };
        }
    }
}
