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
                .ThenInclude(f => f.User)
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
                                               .Include(r => r.Form) 
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

        [HttpPost("{id}/answers/")]
        public async Task<AnswerMultipleChoice> PostAnswerMultipleChoice(CreateAnswerMultipleChoiceDTO createAnswerMultipleChoiceDTO)
        {
            var answerMultipleChoice = new AnswerMultipleChoice(createAnswerMultipleChoiceDTO);
            answerMultipleChoice.ResponseId = createAnswerMultipleChoiceDTO.ResponseId;
            answerMultipleChoice.QuestionId = createAnswerMultipleChoiceDTO.QuestionId;

            _context.Answers.Add(answerMultipleChoice);
            await _context.SaveChangesAsync();

            if (createAnswerMultipleChoiceDTO.OptionsId != null)
            {
                foreach (var optionId in createAnswerMultipleChoiceDTO.OptionsId)
                {
                    var answeredOption = new AnsweredOption
                    {
                        AnswerMultipleChoiceId = answerMultipleChoice.Id,
                        OptionId = optionId
                    };
                    _context.AnsweredOptions.Add(answeredOption);
                }
            }

            await _context.SaveChangesAsync();

            return answerMultipleChoice;
        }
    }
}
