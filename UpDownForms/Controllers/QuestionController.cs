using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.QuestionDTOs;

namespace UpDownForms.Controllers
{
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
    }
}
