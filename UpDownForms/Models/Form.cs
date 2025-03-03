using System.ComponentModel.DataAnnotations.Schema;
using UpDownForms.DTO.FormDTOs;
using UpDownForms.DTO.User;

namespace UpDownForms.Models
{
    [Table("Forms")]
    public class Form
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }
        public User User { get; set; }
        public List<Question> Questions { get; set; }
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
