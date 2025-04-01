using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UpDownForms.DTO.FormDTOs;
using UpDownForms.DTO.ResponseDTOs;
using UpDownForms.DTO.UserDTOs;
using UpDownForms.Models;
using UpDownForms.Pagination;
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

        public async Task<Pageable<FormDTO>> GetForms(PageParameters pageParameters)
        {
            var response = _context.Forms
                                   .Include(f => f.User)
                                   .Include(f => f.Questions)
                                   //.ThenInclude(q => (q as QuestionMultipleChoice).Options)
                                   .Where(f => !f.IsDeleted);
                                   //.AsQueryable();
                                   //.ToListAsync();
                                         
            if (response == null)
            {
                throw new EntityNotFoundException();
            }
            //var listDTO = response.Select(f => f.ToFormDTO()).ToList();
            //Pageable<FormDTO> teste = new Pageable<FormDTO>(listDTO, pageSize, pageIndex, itemsCount);
            //return await Pageable<FormDTO>.ToPageable(listDTO, pageSize, pageIndex);
            var pageable = await Pageable<FormDTO>.ToPageable(response.Select(f => f.ToFormDTO()), pageParameters.PageSize, pageParameters.Page);
            if (pageable.Items.Count() == 0)
            {
                throw new EntityNotFoundException();
            }
            return pageable;
            //return teste;
            //return new Pageable<FormDTO>.ToPageable(listDTO, pageSize, pageIndex, itemsCount);
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
            }

            var userId = _userService.GetLoggedInUserId();
            if (userId == null)
            {
                throw new UnauthorizedException("User must be logged");
            }
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException("Invalid user Id");

            }
            try
            {
                var form = new Form(createFormDTO, userId);

                _context.Forms.Add(form);
                await _context.SaveChangesAsync();
                return form.ToFormDTO();
            }
            catch (DbUpdateException ex)
            {
                throw new BadHttpRequestException(ex.InnerException.Message);
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

        public async Task<Pageable<ResponseDTO>> GetResponsesByFormId(int id, PageParameters pageParameters)
        {
            var formResponses = _context.Responses.Include(r => r.Answers.OrderBy(a => a.QuestionId)).Where(r => r.FormId == id).Where(r => r.Answers.Count() != 0).Select(r => r.ToResponseDTO());
            if (formResponses == null)
            {
                throw new EntityNotFoundException();
            }

            var pageable = await Pageable<ResponseDTO>.ToPageable(formResponses, pageParameters.PageSize, pageParameters.Page);
            if (pageable.Items.Count() == 0)
            {
                throw new EntityNotFoundException();
            }
            return pageable;
        }
    }
}



