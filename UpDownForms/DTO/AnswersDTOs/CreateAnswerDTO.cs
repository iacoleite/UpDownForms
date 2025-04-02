using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UpDownForms.DTO.OptionDTOs;
using UpDownForms.DTO.QuestionDTOs;
using UpDownForms.Models;

namespace UpDownForms.DTO.AnswersDTOs
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")] // lowercase because Json
    [JsonDerivedType(typeof(CreateAnswerMultipleChoiceDTO), typeDiscriminator: "MultipleChoice")]
    [JsonDerivedType(typeof(CreateAnswerOpenEndedDTO), typeDiscriminator: "OpenEnded")]
    public abstract class CreateAnswerDTO
    {
        //public int Id { get; set; }
        //public int ResponseId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public string Type{ get; set; }
        //public bool IsDeleted { get; set; }
        
    }

    public class CreateAnswerMultipleChoiceDTO : CreateAnswerDTO
    {
        //public int OptionId { get; set; }
        public List<int> SelectedOptions { get; set; } = new List<int>();

        public CreateAnswerMultipleChoiceDTO()
        {
            Type = "MultipleChoice";
        }
    }

    public class CreateAnswerOpenEndedDTO : CreateAnswerDTO
    {
        [Required(AllowEmptyStrings = false)]

        public string AnswerText { get; set; }

        public CreateAnswerOpenEndedDTO()
        {
            Type = "OpenEnded";
        }
    }
}
