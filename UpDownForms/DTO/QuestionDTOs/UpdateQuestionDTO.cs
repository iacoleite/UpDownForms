using System.Text.Json.Serialization;
using UpDownForms.Models;

namespace UpDownForms.DTO.QuestionDTOs
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")] // it's lowercase because Json
    [JsonDerivedType(typeof(UpdateQuestionMultipleChoiceDTO), typeDiscriminator: "MultipleChoice")]
    [JsonDerivedType(typeof(UpdateQuestionOpenEndedDTO), typeDiscriminator: "OpenEnded")]
    public class UpdateQuestionDTO
    {
        public string Text { get; set; }
        public int Order { get; set; }
        //public QuestionType Type { get; set; }
        public bool IsRequired { get; set; }
    }
}