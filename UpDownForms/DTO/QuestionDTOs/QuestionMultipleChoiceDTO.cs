using System.Text.Json.Serialization;
using UpDownForms.DTO.OptionDTOs;
using UpDownForms.Models;

namespace UpDownForms.DTO.QuestionDTOs
{
    public class QuestionMultipleChoiceDTO : QuestionDTO
    {
        public bool HasCorrectAnswer { get; set; }
        public QuestionType QuestionType { get; set; }
        [JsonPropertyName("options")]
        public List<OptionDTO> Options { get; set; } = new List<OptionDTO>();
    }
}
