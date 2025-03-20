using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UpDownForms.DTO.FormDTOs;
using UpDownForms.DTO.UserDTOs;

namespace UpDownForms.Models
{
    [Table("Forms")]
    public record Form
    {
        private int _id;
        private string _userId;
        private string _title;
        private string _description;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        private bool _isPublished;
        private bool _isDeleted;
        private User _user;
        private List<Question> _questions = new List<Question>();
        private List<Response> _responses = new List<Response>();

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        [Required]
        public string UserId
        {
            get => _userId;
            set => _userId = value;
        }

        [Required]
        public string Title
        {
            get => _title;
            set => _title = value;
        }

        [Required]
        public string Description
        {
            get => _description;
            set => _description = value;
        }

        [Required]
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
        }

        [Required]
        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set => _updatedAt = value;
        }

        [Required]
        public bool IsPublished
        {
            get => _isPublished;
            set => _isPublished = value;
        }

        [Required]
        public bool IsDeleted
        {
            get => _isDeleted;
            set => _isDeleted = value;
        }

        [Required]
        public User User
        {
            get => _user;
            set => _user = value;
        }

        [Required]
        public List<Question> Questions
        {
            get => _questions;
            set => _questions = value;
        }

        [Required]
        public List<Response> Responses
        {
            get => _responses;
            set => _responses = value;
        }


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
