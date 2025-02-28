using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.Form;
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
        public async Task<ActionResult<IEnumerable<Form>>> GetForms()
        {
            // Linq query to get all forms
            return await _context.Forms.Where(f => !f.IsDeleted).ToListAsync();
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
            return form.formDTO();
        }

        [HttpPost]
        public async Task<ActionResult<Form>> PostForm([FromBody] CreateFormDTO createFormDTO)
        {
            if (createFormDTO == null)
            {
                return BadRequest("Missing form data");
            }

            var form = new Form(createFormDTO);

            // NEED TO FIX THE USER ID, SHOULD GET ID OF THE LOGGED USER !!!
            form.UserId = 10;

            form.CreatedAt = DateTime.UtcNow;
            form.UpdatedAt = DateTime.UtcNow;
            var user = await _context.Users.FindAsync(form.UserId);
            if (user == null)
            {
                return BadRequest("Invalid user ID");
            }
            form.User = user;
            _context.Forms.Add(form);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetForm), new { id = form.Id }, form);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Form>> PutForm(int id, [FromBody] UpdateFormDTO updateFormDTO)
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
            return Ok(form);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Form>> DeleteForm(int id)
        {
            var form = await _context.Forms.FindAsync(id);
            if (form == null)
            {
                return NotFound();
            }
            form.DeleteForm();
            await _context.SaveChangesAsync();
            return Ok($"Form {form.Title} deleted.");
        }

    }
}
