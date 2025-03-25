using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.ApiResponse;
using UpDownForms.DTO.FormDTOs;
using UpDownForms.DTO.UserDTOs;
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

        public FormService()
        {
        }

        public async Task<IEnumerable<FormDTO>> GetForms()
        {
            var response = await _context.Forms
                                         .Include(f => f.User)
                                         .Include(f => f.Questions)
                                            //.ThenInclude(q => (q as QuestionMultipleChoice).Options)
                                         .Where(f => !f.IsDeleted)
                                         .ToListAsync();
            if (response == null)
            {
                throw new EntityNotFoundException();
            }
            return response.Select(f => f.ToFormDTO()).ToList();
        }

        public async Task<FormDTO> GetForm(int id)
        {
            var form = await _context.Forms
                .Include(f => f.User)
                .Include(f => f.Questions)
                    .ThenInclude(q => (q as QuestionMultipleChoice).Options)
                .FirstOrDefaultAsync(f => f.Id == id);
            if (form == null)
            {
                 throw new EntityNotFoundException();
            }
            return form.ToFormDTO();
        }

        public async Task<FormDTO> PostForm(CreateFormDTO createFormDTO)
        {
            if (createFormDTO == null)
            {
                throw new BadHttpRequestException("Missing input data");
                //return new ApiResponse<FormDTO>(false, "Missing form data", null);
            }

            var userId = _userService.GetLoggedInUserId();
            if (userId == null)
            {
                throw new UnauthorizedException("User must be logged");
                //return new ApiResponse<FormDTO>(false, "User should be logged", null);
            }
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException("Invalid user Id");

                //return new ApiResponse<FormDTO>(false, "Invalid user ID", null);
            }
            try
            {
                var form = new Form(createFormDTO, userId);

                _context.Forms.Add(form);
                await _context.SaveChangesAsync();
                //return new ApiResponse<FormDTO>(true, "Form created successfully", form.ToFormDTO());
                return form.ToFormDTO();
            } 
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<FormDTO> PutForm(int id, UpdateFormDTO updateFormDTO)
        {
            var form = await _context.Forms
                                     .Include(f => f.User)
                                     .Include(f => f.Questions)
                                     .Include(f => f.Responses)
                                     .FirstOrDefaultAsync(f => f.Id == id);
            if (form == null)
            {
                throw new EntityNotFoundException("Form not found");
            }
            var userId = _userService.GetLoggedInUserId();
            if (!(form.User.Id == userId))
            {
                throw new UnauthorizedException("You are not authorized to update this form");
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
            return form.ToFormDTO();
        }

        public async Task<FormDTO> DeleteForm(int id)
        {
            var form = await _context.Forms
                                     .Include(f => f.User)
                                     .Include(f => f.Questions)
                                     .Include(f => f.Responses)
                                     .FirstOrDefaultAsync(f => f.Id == id);
            if (form == null)
            {
                throw new EntityNotFoundException("Form not found");
            }
            var user = form.User;
            var loggedUserId = _userService.GetLoggedInUserId();
            if (user.Id != loggedUserId)
            {
                throw new UnauthorizedException("You are not authorized to update this form");
            }

            //var form = await _context.Forms.Include(f => f.User).Include(f => f.Questions).Include(f => f.Responses).FindAsync(id);
            form.IsDeleted = true;
            _context.Forms.Update(form);
            await _context.SaveChangesAsync();
            return form.ToFormDTO();
        }
    }
}



