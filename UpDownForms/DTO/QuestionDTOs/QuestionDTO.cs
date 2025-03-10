using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.OptionDTOs;
using UpDownForms.Models;

namespace UpDownForms.DTO.QuestionDTOs
{
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