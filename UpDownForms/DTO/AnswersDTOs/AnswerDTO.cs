using System.Text.Json.Serialization;
using UpDownForms.DTO.QuestionDTOs;
using UpDownForms.Models;

namespace UpDownForms.DTO.AnswersDTOs
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")] // it's lowercase because Json
    [JsonDerivedType(typeof(AnswerMultipleChoiceResponseDTO), typeDiscriminator: "MultipleChoice")]
    [JsonDerivedType(typeof(AnswerOpenEndedResponseDTO), typeDiscriminator: "OpenEnded")]
    public class AnswerDTO
    {
        public int Id { get; set; }
        public int ResponseId { get; set; }
        public int QuestionId { get; set; }
        //public string? AnswerText { get; set; }
        //public int? OptionId { get; set; }
        public bool IsDeleted { get; set; }

    }
}
