using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.ResponseDTOs;
using UpDownForms.Models;

namespace UpDownForms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResponseController : Controller
    {
        private readonly UpDownFormsContext _context;

        public ResponseController(UpDownFormsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseDTO>>> GetResponses()
        {
            var responses = await _context.Responses
                .Include(r => r.Form)
                .ThenInclude(f => f.User)
                .Include(r => r.Answers)
                .Where(r => !r.IsDeleted)
                .ToListAsync();

            return Ok(responses.Select(response => response.ToResponseDTO()).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDTO>> GetResponse(int id)
        {
            var response = await _context.Responses
                .Include(r => r.Form)
                .ThenInclude(f => f.User) // Ensure User is included
                .Include(r => r.Answers)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response.ToResponseDTO());
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> PostResponse([FromBody] CreateResponseDTO createResponseDTO)
        {
            if (createResponseDTO == null)
            {
                return BadRequest("Missing response data");
            }
            var formExists = await _context.Forms.AnyAsync(f => f.Id == createResponseDTO.FormId);
            if (!formExists)
            {
                return BadRequest("Invalid FormId");
            }
            var response = new Response(createResponseDTO);
            _context.Responses.Add(response);
            await _context.SaveChangesAsync();
            var responseEntity = await _context.Responses
                                               .Include(r => r.Form) // Ensure Form is included
                                               .ThenInclude(f => f.User)
                                               .FirstOrDefaultAsync(r => r.Id == response.Id);
            if (responseEntity == null)
            {
                return BadRequest("Can't find response in db");
            }
            var responseDTO = responseEntity.ToResponseDTO();
            if (responseDTO == null)
            {
                return BadRequest("Can't find response in db");
            }
            return CreatedAtAction(nameof(GetResponse), new { id = response.Id }, responseDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteResponse(int id)
        {
            var response = await _context.Responses.FindAsync(id);
            if (response == null)
            {
                return NotFound();
            }
            response.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Ok("Response deleted");
        }

        [HttpPost("{id}/answers")]
        public async Task<ActionResult> PostAnswer(int id, [FromBody] CreateAnswerDTO createAnswerDTO)
        {
            if (createAnswerDTO == null)
            {
                return BadRequest("Missing answer data");
            }

            var response = await _context.Responses
                .Include(r => r.Answers)
                .Include(r => r.Form)
                .ThenInclude(f => f.User)
                .Include(r => r.Form)
                .ThenInclude(f => f.Questions)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (response == null)
            {
                return NotFound();
            }

            var question = response.Form.Questions.FirstOrDefault(q => q.Id == createAnswerDTO.QuestionId);
            if (question == null)
            {
                return BadRequest("Invalid question ID");
            }

            if (question.Type == QuestionType.MultipleChoice || question.Type == QuestionType.Checkbox || question.Type == QuestionType.Dropdown)
            {
                if (!createAnswerDTO.OptionId.HasValue)
                {
                    return BadRequest("Option ID is required for this question type");
                }

                var options = await _context.Options.AnyAsync(o => o.Id == createAnswerDTO.OptionId);
                if (!options)
                {
                    return BadRequest("Invalid option ID");
                }
            }

            var answer = new Answer(createAnswerDTO);
            response.Answers.Add(answer);
            await _context.SaveChangesAsync();
            var responseEntity = await _context.Responses
                .Include(r => r.Answers)
                .Include(r => r.Form)
                .ThenInclude(f => f.User)
                .Include(r => r.Form)
                .ThenInclude(f => f.Questions)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (responseEntity == null)
            {
                return BadRequest("Can't find response in db");
            }
            var responseDTO = responseEntity.ToResponseDTO();
            return CreatedAtAction(nameof(GetResponse), new { id = response.Id }, responseDTO);
        }
    }
}
