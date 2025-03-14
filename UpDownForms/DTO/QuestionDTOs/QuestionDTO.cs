using System.Text.Json.Serialization;
using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.OptionDTOs;
using UpDownForms.Models;

namespace UpDownForms.DTO.QuestionDTOs
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")] // it's lowercase because Json
    [JsonDerivedType(typeof(QuestionMultipleChoiceDTO), typeDiscriminator: "MultipleChoice")]
    //[JsonDerivedType(typeof(QuestionOpenEndedDTO), typeDiscriminator: "OpenEnded")]
    [JsonDerivedType(typeof(BaseQuestionDTO), typeDiscriminator: "OpenEnded")]
    public abstract class QuestionDTO
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
        public bool IsDeleted { get; set; }
        //public List<OptionDTO> Options { get; set; }
        public List<AnswerDTO> Answers { get; set; }
    }
}