using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UpDownForms.DTO.FormDTOs;
using UpDownForms.DTO.UserDTOs;

namespace UpDownForms.Models
{
    [Table("Forms")]
    public class Form : IVerifyOwnership
    {
        public int Id { get;  set; }
        [Required]
        public string UserId { get;  set; }
        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Title { get;   set; }
        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
        [Required]
        public bool IsPublished { get;  set; }
        [Required]
        public bool IsDeleted { get;  set; }
        [Required]
        public User User { get;  set; }
        [Required]
        public List<Question> Questions { get; set; } = new List<Question>();
        [Required]
        public List<Response> Responses { get; set; } = new List<Response>();

        public Form()
        {
        }

        public Form(CreateFormDTO createFormDTO, string userId)
        {
            this.Title = createFormDTO.Title;
            this.Description = createFormDTO.Description;
            this.UserId = userId;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }

        public FormDTO ToFormDTO()
        {
            return new FormDTO
            {
                Id = this.Id,
                UserId = this.UserId,
                Title = this.Title,
                Description = this.Description,
                CreatedAt = this.CreatedAt,
                UpdatedAt = this.UpdatedAt,
                IsPublished = this.IsPublished,
                IsDeleted = this.IsDeleted,
                User = User.ToUserDetailsDTO(),
                Questions = this.Questions != null ? this.Questions.Select(q => q.ToQuestionDTO()).ToList() : null,
                //Responses = this.Responses != null ? this.Responses.Select(r => r.ToResponseDTO()).ToList() : null
            };
        }
        public FormNoResponsesDTO ToFormNoResponsesDTO()
        {
            return new FormNoResponsesDTO
            {
                Id = this.Id,
                UserId = this.UserId,
                Title = this.Title,
                Description = this.Description,
                CreatedAt = this.CreatedAt,
                UpdatedAt = this.UpdatedAt,
                IsPublished = this.IsPublished,
                IsDeleted = this.IsDeleted,
                User = User.ToUserDetailsDTO(),
                Questions = this.Questions != null ? this.Questions.Select(q => q.ToQuestionDTO()).ToList() : null,
                //Responses = this.Responses != null ? this.Responses.Select(r => r.ToResponseDTO()).ToList() : null
            };
        }
        public FormNoQuestionsDTO ToFormNoQuestionDTO()
        {
            return new FormNoQuestionsDTO
            {
                Id = this.Id,
                UserId = this.UserId,
                Title = this.Title,
                Description = this.Description,
                CreatedAt = this.CreatedAt,
                UpdatedAt = this.UpdatedAt,
                IsPublished = this.IsPublished,
                IsDeleted = this.IsDeleted,
                User = User.ToUserDetailsDTO(),
                //Questions = this.Questions != null ? this.Questions.Select(q => q.ToQuestionDTO()).ToList() : null,
                Responses = this.Responses != null ? this.Responses.Select(r => r.ToResponseDTO()).ToList() : null
            };
        }        
        public FormDetailsDTO ToFormDetailsDTO()
        {
            return new FormDetailsDTO
            {
                Id = this.Id,
                UserId = this.UserId,
                Title = this.Title,
                Description = this.Description,
                CreatedAt = this.CreatedAt,
                UpdatedAt = this.UpdatedAt,
                IsPublished = this.IsPublished,
                IsDeleted = this.IsDeleted,
                //User = User.ToUserDetailsDTO(),
                //Questions = this.Questions != null ? this.Questions.Select(q => q.ToQuestionDTO()).ToList() : null,
                //Responses = this.Responses != null ? this.Responses.Select(r => r.ToResponseDTO()).ToList() : null
            };
        }

        internal void UpdateForm(UpdateFormDTO updateFormDTO)
        {
            this.Title = updateFormDTO.Title;
            this.Description = updateFormDTO.Description;
            this.UpdatedAt = DateTime.UtcNow;
        }

        internal void DeleteForm()
        {
            this.IsDeleted = true;
        }
    }
}
