using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UpDownForms.DTO.FormDTOs;
using UpDownForms.DTO.UserDTOs;

namespace UpDownForms.Models
{
    [Table("Forms")]
    public class Form
    {

        protected internal int Id { get; private set; }
        [Required]
        public string UserId { get; private set; }
        [Required(AllowEmptyStrings = false)]
        public string Title { get; protected internal set; }
        [Required(AllowEmptyStrings = false)]
        public string Description { get; protected internal set; }
        [Required]
        public DateTime CreatedAt { get; protected internal set; }
        [Required]
        public DateTime UpdatedAt { get; protected internal set; }
        [Required]
        public bool IsPublished { get; protected set; }
        [Required]
        public bool IsDeleted { get;  protected internal set; }
        [Required]
        public User User { get; private set; }
        [Required]
        public List<Question> Questions { get; protected set; }
        [Required]
        public List<Response> Responses { get; protected set; }

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
