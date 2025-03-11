using System.Text.Json.Serialization;
using UpDownForms.DTO.OptionDTOs;
using UpDownForms.Models;

namespace UpDownForms.DTO.QuestionDTOs
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")] // Tell System.Text.Json to use "type" property for discrimination
    [JsonDerivedType(typeof(CreateQuestionMultipleChoiceDTO), typeDiscriminator: "MultipleChoice")] // Register MultipleChoice DTO and discriminator value
    [JsonDerivedType(typeof(CreateQuestionOpenEndedDTO), typeDiscriminator: "OpenEnded")]     // Register OpenEnded DTO and discriminator value
    public abstract class CreateQuestionDTO
    {
        public int FormId { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }

        public CreateQuestionDTO() { }
    }

    public class CreateQuestionMultipleChoiceDTO : CreateQuestionDTO
    {
        public List<CreateOptionDTO> Options { get; set; } = new List<CreateOptionDTO>();
        public bool HasCorrectAnswer { get; set; }
        public string QuestionType { get; set; }

        public CreateQuestionMultipleChoiceDTO() 
        {
            Type = "MultipleChoice";
        }
    }

    public class CreateQuestionOpenEndedDTO : CreateQuestionDTO
    {
        public CreateQuestionOpenEndedDTO() 
        {
            Type = "OpenEnded";
        }
        //public string Answer { get; set; }
    }
}