using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.ApiResponse;
using UpDownForms.DTO.FormDTOs;
using UpDownForms.Models;
using UpDownForms.Security;

namespace UpDownForms.Services
{
    public class FormService
    {
        private readonly UpDownFormsContext _context;
        private readonly IPasswordHelper _passwordHelper;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public FormService(UpDownFormsContext context, IPasswordHelper passwordHelper, UserManager<User> userManager, IUserService userService)
        {
            _context = context;
            _passwordHelper = passwordHelper;
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<ApiResponse<IEnumerable<FormDTO>>> GetForms()
        {
            var response = await _context.Forms
                                         .Include(f => f.User)
                                         .Include(f => f.Questions)
                                         .ThenInclude(q => (q as QuestionMultipleChoice).Options)
                                         .Where(f => !f.IsDeleted)
                .ToListAsync();
            return new ApiResponse<IEnumerable<FormDTO>>(true, "ok!", response.Select(f => f.ToFormDTO()).ToList());
        }

        public async Task<ApiResponse<FormDTO>> GetForm(int id)
        {
            var form = await _context.Forms.Include(f => f.User).Include(f => f.Questions).FirstOrDefaultAsync(f => f.Id == id);
            if (form == null)
            {
                return new ApiResponse<FormDTO>(false, "Form not found", null);
            }
            return new ApiResponse<FormDTO>(true, "OK", form.ToFormDTO());
        }

        public async Task<ApiResponse<FormDTO>> PostForm(CreateFormDTO createFormDTO)
        {
            if (createFormDTO == null)
            {
                return new ApiResponse<FormDTO>(false, "Missing form data", null);
            }

            var userId = _userService.GetLoggedInUserId();
            if (userId == null)
            {
                return new ApiResponse<FormDTO>(false, "User should be logged", null);
            }
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return new ApiResponse<FormDTO>(false, "Invalid user ID", null);
            }
            var form = new Form(createFormDTO, userId);

            _context.Forms.Add(form);
            await _context.SaveChangesAsync();
            return new ApiResponse<FormDTO>(true, "Form created successfully", form.ToFormDTO());
        }

        public async Task<ApiResponse<FormDTO>> PutForm(int id, UpdateFormDTO updateFormDTO)
        {
            var form = await _context.Forms
                                     .Include(f => f.User)
                                     .Include(f => f.Questions)
                                     .Include(f => f.Responses)
                                     .FirstOrDefaultAsync(f => f.Id == id);
            if (form == null)
            {
                return new ApiResponse<FormDTO>(false, "Form not found", null);
            }
            var userId = _userService.GetLoggedInUserId();
            if (!(form.User.Id == userId))
            {
                return new ApiResponse<FormDTO>(false, "You are not authorized to update this form", null);
            }
            if (!string.IsNullOrEmpty(updateFormDTO.Title))
            {
                form.Title = updateFormDTO.Title;
            }
            if (!string.IsNullOrEmpty(updateFormDTO.Description))
            {
                form.Description = updateFormDTO.Description;
            }
            form.UpdatedAt = DateTime.UtcNow;
            form.IsDeleted = false;
            _context.Forms.Update(form);
            await _context.SaveChangesAsync();
            return new ApiResponse<FormDTO>(true, "Form updated successfully", form.ToFormDTO());
        }

        public async Task<ApiResponse<FormDTO>> DeleteForm(int id)
        {
            var form = await _context.Forms
                                     .Include(f => f.User)
                                     .Include(f => f.Questions)
                                     .Include(f => f.Responses)
                                     .FirstOrDefaultAsync(f => f.Id == id);
            if (form == null)
            {
                return new ApiResponse<FormDTO>(false, "Form not found", null);
            }

            //var form = await _context.Forms.Include(f => f.User).Include(f => f.Questions).Include(f => f.Responses).FindAsync(id);
            form.IsDeleted = true;
            _context.Forms.Update(form);
            await _context.SaveChangesAsync();
            return new ApiResponse<FormDTO>(true, "Form deleted successfully", form.ToFormDTO());
        }
    }
}



