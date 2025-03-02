using UpDownForms.Models;

namespace UpDownForms.DTO.QuestionDTOs
{
    public class QuestionDetailsDTO
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public QuestionType Type { get; set; }
        public bool IsRequired { get; set; }
        public bool IsDeleted { get; set; }
        //public List<OptionDTO> Options { get; set; }
    }
}