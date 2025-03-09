namespace UpDownForms.DTO.OptionDTOs
{
    public class OptionDTO
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsCorrect { get; set; }
    }
}
