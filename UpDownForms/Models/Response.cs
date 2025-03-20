using System.ComponentModel.DataAnnotations.Schema;
using UpDownForms.DTO.ResponseDTOs;

namespace UpDownForms.Models
{
    [Table("Responses")]
    public class Response
    {
        private int _id;
        private int _formId;
        private string? _respondentEmail;
        private DateTime _submittedAt;
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

        public string? RespondentEmail
        {
            get => _respondentEmail;
            set => _respondentEmail = value;
        }

        public DateTime SubmittedAt
        {
            get => _submittedAt;
            set => _submittedAt = value;
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
            try
            {
                return new ResponseDTO
                {
                    Id = this.Id,
                    FormId = this.FormId,
                    RespondentEmail = this.RespondentEmail,
                    SubmittedAt = this.SubmittedAt,
                    IsDeleted = this.IsDeleted,
                    Form = this.Form != null ? this.Form.ToFormDetailsDTO() : null,
                    Answers = this.Answers != null ? this.Answers.Select(a => a.ToAnswerDTO()).ToList() : null
                };
            }
            catch (Exception e )
            {
                throw new Exception("Error converting Response to ResponseDTO", e);
            }
        }
        
        public ResponseFormNoResponseDTO ToResponseFormNoResponseDTO()
        {
            return new ResponseFormNoResponseDTO
            {
                Id = this.Id,
                FormId = this.FormId,
                RespondentEmail = this.RespondentEmail,
                SubmittedAt = this.SubmittedAt,
                IsDeleted = this.IsDeleted,
                //Form = this.Form != null ? this.Form.ToFormNoResponsesDTO() : null,
                Answers = this.Answers != null ? this.Answers.Select(a => a.ToAnswerDTO()).ToList() : null
            };
        }

        public void DeleteResponse()
        {
            this.IsDeleted = true;
            foreach (Answer a in Answers)
            {
                a.IsDeleted = true;
            }
        }


    }
}
