using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UpDownForms.DTO.FormDTOs;
using UpDownForms.DTO.UserDTOs;

namespace UpDownForms.Models
{
    [Table("Forms")]
    public record Form
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
        [Required]
        public bool IsPublished { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public List<Question> Questions { get; set; }
        [Required]
        public List<Response> Responses { get; set; }
        //public string PasswordHash { get; set; }

        public Form()
        {
        }

        public Form(CreateFormDTO createFormDTO)
        {
            this.Title = createFormDTO.Title;
            this.Description = createFormDTO.Description;

            // NEED TO FIX THE USER ID, SHOULD GET ID OF THE LOGGED USER !!!
            this.UserId = "1";

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
                User = User.ToUserDetailsDTO()
                //User = new UserDetailsDTO
                //{
                //    Id = this.User.Id,
                //    Name = this.User.Name,
                //    Email = this.User.Email,
                //    //PasswordHash = this.User.PasswordHash,
                //    CreatedAt = this.User.CreatedAt,
                //    IsDeleted = this.User.IsDeleted
                //}
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
