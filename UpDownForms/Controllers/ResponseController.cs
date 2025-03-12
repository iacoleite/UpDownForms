using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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
            try
            {
                var responses = await _context.Responses
                    .Include(r => r.Form)
                    .ThenInclude(f => f.User)
                    .Include(r => r.Answers)
                    .Where(r => !r.IsDeleted)
                    .ToListAsync();

                return Ok(responses.Select(response => response.ToResponseDTO()).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseFormNoResponseDTO>> GetResponse(int id)
        {
            var response = await _context.Responses
                .Include(r => r.Form)
                    .ThenInclude(f => f.User)
                .Include(r => r.Answers)
                .Include(r => r.Form)
                    .ThenInclude(f => f.Questions)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response.ToResponseFormNoResponseDTO());
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
        public async Task<ActionResult<AnswerDTO>> PostAnswer(int id, [FromBody] CreateAnswerDTO createAnswerDTO)
        {
            var response = await _context.Responses.FindAsync(id);
            
            if (createAnswerDTO == null)
            {
                return BadRequest("missing answer data");
            }
            
            if (response == null)
            {
                return BadRequest("Invalid ResponseId");
            }
            Answer answer;
            if (createAnswerDTO is CreateAnswerOpenEndedDTO answerOpenEndedDTO)
            {
                answer = new AnswerOpenEnded(answerOpenEndedDTO);
                ((AnswerOpenEnded)answer).AnswerText = answerOpenEndedDTO.AnswerText;
                answer.ResponseId = id;
                answer.QuestionId = createAnswerDTO.QuestionId;
                await _context.Answers.AddAsync(answer);
            }
            else if (createAnswerDTO is CreateAnswerMultipleChoiceDTO createAnswerMultipleChoiceDTO)
            {
                answer = new AnswerMultipleChoice(createAnswerMultipleChoiceDTO);
                AnswerMultipleChoice answerMultipleChoice = (AnswerMultipleChoice)answer;

                answerMultipleChoice.ResponseId = id;
                answerMultipleChoice.QuestionId = createAnswerMultipleChoiceDTO.QuestionId;
                await _context.AnswersMultipleChoice.AddAsync(answerMultipleChoice);
                await _context.SaveChangesAsync();

                if (createAnswerMultipleChoiceDTO != null && createAnswerMultipleChoiceDTO.SelectedOptions.Any())
                {
                    foreach (var optionId in createAnswerMultipleChoiceDTO.SelectedOptions.Distinct())
                    {
                        try
                        {
                            //var answeredOption = new AnsweredOption
                            var existingAnsweredOption = await _context.AnsweredOptions.FirstOrDefaultAsync(ao => ao.OptionId == optionId && ao.AnswerMultipleChoiceId == answerMultipleChoice.Id);
                            if (existingAnsweredOption == null)
                            {
                                var answeredOption = new AnsweredOption
                                {
                                    AnswerMultipleChoiceId = answerMultipleChoice.Id,
                                    OptionId = optionId
                                };
                                await _context.AnsweredOptions.AddAsync(answeredOption);
                            }
                        }
                        catch (Exception e)
                        {
                            return BadRequest(e.Message);
                        }
                    }
                    await _context.SaveChangesAsync();

                }
                answer = answerMultipleChoice;
            }
            else
            {
                return BadRequest("Invalid answer type");
            }

            if (answer != null && !(answer is AnswerMultipleChoice))
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetResponse), new { id = answer.Id }, ((AnswerOpenEnded)answer).ToAnswerOpenEndedResponseDTO());
            }
            else if (answer != null)
            {
                return CreatedAtAction(nameof(GetResponse), new { id = answer.Id }, answer.ToAnswerDTO());
            }
            else
            {
                return BadRequest("Can't create answer");
            }
        }
    }
}
