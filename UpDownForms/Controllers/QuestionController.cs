using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.OptionDTOs;
using UpDownForms.DTO.QuestionDTOs;
using UpDownForms.Models;
using UpDownForms.Services;

namespace UpDownForms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : Controller
    {
        private readonly UpDownFormsContext _context;
        private readonly IUserService _userService;

        public QuestionController(UpDownFormsContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDetailsDTO>>> GetQuestions()
        {
            var questions = await _context.Questions.Include(q => q.Form).Where(q => !q.IsDeleted).ToListAsync();
            return Ok(questions.Select(q => q.ToQuestionDetailsDTO()).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDTO>> GetQuestion(int id)
        {
            var question = await _context.Questions.Include(q => q.Form).Include(q => q.Answers).Include(q => (q as QuestionMultipleChoice).Options).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            QuestionDTO questionDTO = question.ToQuestionDTO();
            //if (questionDTO is QuestionMultipleChoiceDTO questionMultipleChoiceDTO)
            //{
            //    (QuestionMultipleChoiceDTO)questionDTO. = (QuestionMultipleChoice)question
            //}
            if (question is QuestionMultipleChoice multipleChoiceQuestion)
            {
                QuestionMultipleChoiceDTO multipleChoiceDTO = (QuestionMultipleChoiceDTO)multipleChoiceQuestion.ToQuestionDTO();
                multipleChoiceQuestion.Options = ((QuestionMultipleChoice)question).Options;
                questionDTO = multipleChoiceDTO;
            }
            else
            {
                questionDTO = question.ToQuestionDTO();
            }
            questionDTO.Answers = question.Answers.Select(a => a.ToAnswerDTO()).ToList();
            
            return Ok(questionDTO);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<QuestionDetailsDTO>> PostQuestion([FromBody] CreateQuestionDTO createQuestionDTO)
        {
            if (createQuestionDTO == null)
            {
                return BadRequest("Missing question data");
            }

            var form = await _context.Forms.FindAsync(createQuestionDTO.FormId);
            if (form == null)
            {   
                return BadRequest("Can't find form to add question");
            }

            var userId = _userService.GetLoggedInUserId();
            
            if (!form.UserId.Equals(userId))
            {
                return BadRequest("Logged user does not authorization to post to form");
            }

            Question question;

            if (createQuestionDTO is CreateQuestionMultipleChoiceDTO createQuestionMultipleChoiceDTO)
            {
                question = new QuestionMultipleChoice(createQuestionMultipleChoiceDTO);
                if (createQuestionMultipleChoiceDTO.Options != null && createQuestionMultipleChoiceDTO.Options.Any())
                {
                    foreach (var optionDTO in createQuestionMultipleChoiceDTO.Options)
                    {
                        var option = new Option(optionDTO);
                        ((QuestionMultipleChoice)question).AddOption(option);
                    }
                    ((QuestionMultipleChoice)question).QuestionMCType = Enum.Parse<QuestionType>(createQuestionMultipleChoiceDTO.QuestionType.ToString(), true);
                }
            }
            else if (createQuestionDTO is CreateQuestionOpenEndedDTO createQuestionOpenEndedDTO)
            {
                question = new QuestionOpenEnded(createQuestionOpenEndedDTO);
}
            else 
            {
                return BadRequest("Missing question data");
            }
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, question.ToQuestionDetailsDTO());
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<QuestionDTO>> PutQuestion(int id, [FromBody] UpdateQuestionDTO updateQuestionDTO)
        {
            if (updateQuestionDTO == null)
            {
                return BadRequest("Missing question data");
            }
            var question = await _context.Questions.Include(q => q.Form).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }
            var userId = _userService.GetLoggedInUserId();
            var formId = question.Form.UserId;
            if (!formId.Equals(userId))
            {
                return BadRequest("Logged user does not authorization to update question");
            }

            if (updateQuestionDTO is UpdateQuestionMultipleChoiceDTO updateQuestionMultipleChoiceDTO)
            {
                if (!(question is QuestionMultipleChoice multipleChoiceQuestion))
                {
                    return BadRequest("Question type mismatch");
                }
                multipleChoiceQuestion.Text = updateQuestionMultipleChoiceDTO.Text;
                multipleChoiceQuestion.Order = updateQuestionMultipleChoiceDTO.Order;
                multipleChoiceQuestion.IsRequired = updateQuestionMultipleChoiceDTO.IsRequired;
                multipleChoiceQuestion.HasCorrectAnswer = updateQuestionMultipleChoiceDTO.HasCorrectAnswer;
                //multipleChoiceQuestion.QuestionType = updateQuestionMultipleChoiceDTO.Type;
                multipleChoiceQuestion.IsDeleted = false;
            }
            else if (updateQuestionDTO is UpdateQuestionOpenEndedDTO updateQuestionOpenEndedDTO)
            {
                if (!(question is QuestionOpenEnded openEndedQuestion))
                {
                    return BadRequest("Question type mismatch");
                }
                openEndedQuestion.Text = updateQuestionOpenEndedDTO.Text;
                openEndedQuestion.Order = updateQuestionOpenEndedDTO.Order;
                openEndedQuestion.IsRequired = updateQuestionOpenEndedDTO.IsRequired;
                openEndedQuestion.IsDeleted = false;
            }
            else
            {
                question.Text = updateQuestionDTO.Text;
                question.Order = updateQuestionDTO.Order;
                question.IsRequired = updateQuestionDTO.IsRequired;
            }
            question.IsDeleted = false;
            await _context.SaveChangesAsync();
            return Ok(question.ToQuestionDTO());
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Questions.Include(q => q.Form).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            var userId = _userService.GetLoggedInUserId();
            var formId = question.Form.UserId;
            if (!formId.Equals(userId))
            {
                return BadRequest("Logged user does not authorization to update question");
            }
            question.DeleteQuestion();
            _context.SaveChanges();
            return Ok();
        }


        // Handling options
        [HttpGet("{questionId}/options")]
        public async Task<ActionResult<IEnumerable<OptionDTO>>> GetOptions(int questionId)
        {
            var question = await _context.Questions.OfType<QuestionMultipleChoice>().Include(q => q.Options).FirstOrDefaultAsync(q => q.Id == questionId);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question.Options.Select(o => o.ToOptionDTO()).ToList());
        }

        [Authorize]
        [HttpPost("{questionId}/options")]
        public async Task<IActionResult> AddOption(int questionId, [FromBody] CreateOptionDTO createOptionDTO)
        {
            var question = await _context.Questions.OfType<QuestionMultipleChoice>().Include(q => q.Options).Include(q => q.Form).FirstOrDefaultAsync(q => q.Id == questionId);
            if (question == null)
            {
                return NotFound("Question not found");
            }
            var userId = _userService.GetLoggedInUserId();
            var formId = question.Form.UserId;
            if (!formId.Equals(userId))
            {
                return BadRequest("Logged user does not authorization to create Option to question");
            }
            var option = new Option(createOptionDTO);
            question.AddOption(option);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOptions), new { questionId = questionId }, option.ToOptionDTO());
        }

        [Authorize]
        [HttpDelete("{questionId}/options/{optionId}")]
        public async Task<IActionResult> DeleteOption(int questionId, int optionId)
        {
            var question = await _context.Questions.OfType<QuestionMultipleChoice>().Include(q => q.Options).Include(q => q.Form).FirstOrDefaultAsync(q => q.Id == questionId);
            if (question == null)
            {
                return NotFound("Question not found");
            }
            var option = question.Options.FirstOrDefault(o => o.Id == optionId);
            if (option == null)
            {
                return NotFound("Option not found");
            }
            var userId = _userService.GetLoggedInUserId();
            var formId = question.Form.UserId;
            if (!formId.Equals(userId))
            {
                return BadRequest("Logged user does not authorization to delete option");
            }
            // Should use soft delete here? 
            question.Options.Remove(option);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // implement update?
    }
}
