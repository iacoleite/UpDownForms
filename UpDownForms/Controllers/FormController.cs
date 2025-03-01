using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.FormDTOs;
using UpDownForms.Models;

namespace UpDownForms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FormController : Controller
    {
        private readonly UpDownFormsContext _context;

        public FormController(UpDownFormsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormDTO>>> GetForms()
        {
            // Linq query to get all forms that are not deleted
            var forms = await _context.Forms.Include(f => f.User).Where(f => !f.IsDeleted).ToListAsync();
            
            return forms.Select(forms => forms.ToFormDTO()).ToList();
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
            return form.ToFormDTO();
        }

        [HttpPost]
        public async Task<ActionResult<FormDTO>> PostForm([FromBody] CreateFormDTO createFormDTO)
        {
            if (createFormDTO == null)
            {
                return BadRequest("Missing form data");
            }

            var form = new Form(createFormDTO);
            
            //var user = await _context.Users.FindAsync(form.UserId);
            //if (user == null)
            //{
            //    return BadRequest("Invalid user ID");
            //}
            //form.User = user;
            _context.Forms.Add(form);
            await _context.SaveChangesAsync();
            int id = form.Id;
            form = await _context.Forms.Include(f => f.User).FirstOrDefaultAsync(f => f.Id == id);
            return CreatedAtAction(nameof(GetForm), new { id = form.Id }, form.ToFormDTO());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FormDTO>> PutForm(int id, [FromBody] UpdateFormDTO updateFormDTO)
        {
            if (updateFormDTO == null)
            {
                return BadRequest("Missing form data");
            }
            var form = await _context.Forms.FindAsync(id);
            if (form == null)
            {
                return NotFound("Missing form");
            }
            form.UpdateForm(updateFormDTO);

            await _context.SaveChangesAsync();
            //return forms.Select(forms => forms.ToFormDTO()).ToList();
            
            form = await _context.Forms.Include(f => f.User).FirstOrDefaultAsync(f => f.Id == id);
            return Ok(form.ToFormDTO());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteForm(int id)
        {
            var form = await _context.Forms.FindAsync(id);
            if (form == null)
            {
                return NotFound();
            }
            form.DeleteForm();
            await _context.SaveChangesAsync();
            //form = await _context.Forms.Include(f => f.User).FirstOrDefaultAsync(f => f.Id == id);
            return Ok($"Form {form.Title} deleted.");
        }

    }
}
