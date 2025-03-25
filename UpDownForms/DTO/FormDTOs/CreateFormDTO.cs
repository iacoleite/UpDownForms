using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using UpDownForms.Models;

namespace UpDownForms.DTO.FormDTOs
{
    public record CreateFormDTO
    {
        //public int Id { get; set; }
        //public int UserId { get; set; }
        [Required(AllowEmptyStrings = false)]
        
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false)] 
        public string Description { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
        //public bool IsPublished { get; set; }
        //public bool IsDeleted { get; set; }
        //public User User { get; set; }
        //public List<Question> Questions { get; set; }
        //public List<Response> Responses { get; set; }
    }
}