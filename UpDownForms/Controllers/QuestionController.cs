using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.OptionDTOs;
using UpDownForms.DTO.QuestionDTOs;
using UpDownForms.Models;

namespace UpDownForms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : Controller
    {
        private readonly UpDownFormsContext _context;

        public QuestionController(UpDownFormsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDetailsDTO>>> GetQuestions()
        {
            var questions = await _context.Questions.Include(q => q.Form).Where(q => !q.IsDeleted).ToListAsync();
            // Linq query to get all questions that are not deleted
            return Ok(questions.Select(q => q.ToQuestionDetailsDTO()).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDTO>> GetQuestion(int id)
        {
            var question = await _context.Questions.Include(q => q.Form).Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            var questionDTO = question.ToQuestionDTO();
            //questionDTO.Answers = question.Answers.Select(a => a.ToAnswerDTO()).ToList();
            if (question is QuestionMultipleChoice multipleChoiceQuestion)
            {
                questionDTO.Options = multipleChoiceQuestion.Options.Select(o => o.ToOptionDTO()).ToList();
            }
            questionDTO.Answers = question.Answers.Select(a => a.ToAnswerDTO()).ToList();
            
            return Ok(questionDTO);
        }

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

            var question = new Question(createQuestionDTO);
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, question.ToQuestionDetailsDTO());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<QuestionDetailsDTO>> PutQuestion(int id, [FromBody] UpdateQuestionDTO updateQuestionDTO)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            question.Text = updateQuestionDTO.Text;
            question.Order = updateQuestionDTO.Order;
            question.IsRequired = updateQuestionDTO.IsRequired;
            await _context.SaveChangesAsync();
            return Ok(question.ToQuestionDetailsDTO());
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteQuestion(int id)
        {
            var question = _context.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
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

        [HttpPost("{questionId}/options")]
        public async Task<IActionResult> AddOption(int questionId, [FromBody] CreateOptionDTO createOptionDTO)
        {
            var question = await _context.Questions.OfType<QuestionMultipleChoice>().Include(q => q.Options).FirstOrDefaultAsync(q => q.Id == questionId);
            if (question == null)
            {
                return NotFound("Question not found");
            }
            var option = new Option(createOptionDTO);
            question.AddOption(option);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOptions), new { questionId = questionId }, option.ToOptionDTO());
        }

        [HttpDelete("{questionId}/options/{optionId}")]
        public async Task<IActionResult> DeleteOption(int questionId, int optionId)
        {
            var question = await _context.Questions.OfType<QuestionMultipleChoice>().Include(q => q.Options).FirstOrDefaultAsync(q => q.Id == questionId);
            if (question == null)
            {
                return NotFound("Question not found");
            }
            var option = question.Options.FirstOrDefault(o => o.Id == optionId);
            if (option == null)
            {
                return NotFound("Option not found");
            }
            // Should I use soft delete here? I'll not implement it now
            question.Options.Remove(option);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // WILL NOT IMPLEMENT UPDATE FOR OPTIONS (does it even make sense?)
    }
}
