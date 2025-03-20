using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.QuestionDTOs;

namespace UpDownForms.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")] // it's lowercase because Json
    [JsonDerivedType(typeof(AnswerMultipleChoice), typeDiscriminator: "MultipleChoice")]
    [JsonDerivedType(typeof(AnswerOpenEnded), typeDiscriminator: "OpenEnded")]
    [Table("Answers")]
    public abstract class Answer
    {
        private int _id;
        private int _responseId;
        private int _questionId;
        private bool _isDeleted;
        private Response _response;
        private Question _question;

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public int ResponseId
        {
            get => _responseId;
            set => _responseId = value;
        }

        public int QuestionId
        {
            get => _questionId;
            set => _questionId = value;
        }

        public bool IsDeleted
        {
            get => _isDeleted;
            set => _isDeleted = value;
        }

        public Response Response
        {
            get => _response;
            set => _response = value;
        }

        public Question Question
        {
            get => _question;
            set => _question = value;
        }
        //public string Type { get; set; }

        public Answer()
        {
        }

        public Answer(CreateAnswerDTO createAnswerDTO)
        {
            //this.ResponseId = createAnswerDTO.ResponseId;
            this.QuestionId = createAnswerDTO.QuestionId;
            //this.AnswerText = createAnswerDTO.AnswerText;
            //this.OptionId = createAnswerDTO.OptionId;
            this.IsDeleted = false;
        }
        public virtual AnswerDTO ToAnswerDTO()
        {
            return new AnswerDTO
            {
                Id = this.Id,
                ResponseId = this.ResponseId,
                QuestionId = this.QuestionId,
                //AnswerText = this.AnswerText,
                //OptionId = this.OptionId,
                IsDeleted = this.IsDeleted
            };
        }


    }
}
