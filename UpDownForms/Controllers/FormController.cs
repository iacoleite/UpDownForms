using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.FormDTOs;
using UpDownForms.Models;
using UpDownForms.Services;

namespace UpDownForms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FormController : ControllerBase
    {
        private readonly UpDownFormsContext _context;
        private readonly FormService _formService;

        public FormController(UpDownFormsContext context, FormService formService)
        {
            _context = context;
            _formService = formService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormDTO>>> GetForms()
        {
            // Linq query to get all forms that are not deleted
            var response = await _formService.GetForms();
            //await _context.Forms.Include(f => f.User).Where(f => !f.IsDeleted).ToListAsync();
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FormDTO>> GetForm(int id)
        {

            //var form = await _context.Forms.Include(f => f.User).FirstOrDefault(f => f.Id == id).FindAsync(id);
            var form = await _context.Forms.Include(f => f.User).FirstOrDefaultAsync(f => f.Id == id);

            //var form = await _context.Forms.FindAsync(id);
            if (form == null)
            {
                return NotFound();
            }
            return Ok(form.ToFormDTO());
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<FormDTO>> PostForm([FromBody] CreateFormDTO createFormDTO)
        {
            var response = await _formService.PostForm(createFormDTO);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data.Title);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FormDTO>> PutForm(int id, [FromBody] UpdateFormDTO updateFormDTO)
        {
            var response = await _formService.PutForm(id, updateFormDTO);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data.Title);

            //if (updateFormDTO == null)
            //{
            //    return BadRequest("Missing form data");
            //}
            //var form = await _context.Forms.FindAsync(id);
            //if (form == null)
            //{
            //    return NotFound("Missing form");
            //}
            //form.UpdateForm(updateFormDTO);

            //await _context.SaveChangesAsync();
            ////return forms.Select(forms => forms.ToFormDTO()).ToList();

            //form = await _context.Forms.Include(f => f.User).FirstOrDefaultAsync(f => f.Id == id);
            //if (form == null) {
            //    return NotFound();
            //}
            //return Ok(form.ToFormDTO());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteForm(int id)
        {
            var response = await _formService.DeleteForm(id);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Message);
            //var form = await _context.Forms.FindAsync(id);
            //if (form == null)
            //{
            //    return NotFound();
            //}
            //form.DeleteForm();
            //await _context.SaveChangesAsync();
            ////form = await _context.Forms.Include(f => f.User).FirstOrDefaultAsync(f => f.Id == id);
            //return Ok($"Form {form.Title} deleted.");
        }
    }
}
