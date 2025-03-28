using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using UpDownForms.DTO.OptionDTOs;
using UpDownForms.DTO.QuestionDTOs;
using UpDownForms.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UpDownForms.Services
{
    public class QuestionService
    {
        private readonly UpDownFormsContext _context;
        private readonly IUserService _userService;

        public QuestionService(UpDownFormsContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<IEnumerable<QuestionDetailsDTO>> GetQuestions()
        {
            var response = await _context.Questions.Include(q => q.Form).Where(q => !q.IsDeleted).ToListAsync();
            if (response == null)
            {
                throw new EntityNotFoundException();
            }
            return response.Select(q => q.ToQuestionDetailsDTO()).ToList();
        }

        public async Task<QuestionDTO> GetQuestionById(int id)
        {
            var question = await _context.Questions.Include(q => q.Form).Include(q => q.Answers).Include(q => (q as QuestionMultipleChoice).Options).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                throw new EntityNotFoundException("Can't find quesiton");
            }

            QuestionDTO questionDTO = question.ToQuestionDTO();

            if (question is QuestionMultipleChoice questionMultipleChoice)
            {
                QuestionMultipleChoiceDTO questionMultipleChoiceDTO = (QuestionMultipleChoiceDTO)questionMultipleChoice.ToQuestionDTO();
                questionDTO = questionMultipleChoiceDTO;
            }
            else
            {
                questionDTO = question.ToQuestionDTO();
            }
            questionDTO.Answers = question.Answers.Select(a => a.ToAnswerDTO()).ToList();

            return questionDTO;
        }

        public async Task<QuestionDetailsDTO> PostQuestion(CreateQuestionDTO createQuestionDTO)
        {
            if (createQuestionDTO == null)
            {
                throw new BadHttpRequestException("Missing input data");
            }

            var form = await _context.Forms.FindAsync(createQuestionDTO.FormId);
            if (form == null)
            {
                throw new EntityNotFoundException("Can't find form to add question");
            }

            var userId = _userService.GetLoggedInUserId();
            if (userId == null)
            {
                throw new UnauthorizedException("User must be logged");
            }
            if (!form.UserId.Equals(userId))
            {
                throw new UnauthorizedException("User not authorized to add a question to this form");
            }

            Question question;

            if (createQuestionDTO is CreateQuestionMultipleChoiceDTO createQuestionMultipleChoiceDTO)
            {
                
                question = new QuestionMultipleChoice(createQuestionMultipleChoiceDTO);
                if (createQuestionMultipleChoiceDTO.Options.IsNullOrEmpty())
                {
                    throw new BadHttpRequestException("A Multiple Choice Question must have at least one Option when created.");
                }
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
                throw new BadHttpRequestException("Missing input data");
            }
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question.ToQuestionDetailsDTO();
        }

        public async Task<QuestionDTO> PutQuestion(int id, UpdateQuestionDTO updateQuestionDTO)
        {
            if (updateQuestionDTO == null)
            {
                throw new BadHttpRequestException("Missing input data");

            }
            var question = await _context.Questions.Include(q => q.Form).Include(q => (q as QuestionMultipleChoice).Options).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                throw new EntityNotFoundException("Can't find question");
            }
            var userId = _userService.GetLoggedInUserId();
            if (userId == null)
            {
                throw new UnauthorizedException("User must be logged");
            }
            if (!question.Form.UserId.Equals(userId))
            {
                throw new UnauthorizedException("User not authorized to update question");
            }

            if (updateQuestionDTO is UpdateQuestionMultipleChoiceDTO updateQuestionMultipleChoiceDTO)
            {
                if (!(question is QuestionMultipleChoice multipleChoiceQuestion))
                {
                    throw new BadHttpRequestException("Question type mismatch");

                }

                multipleChoiceQuestion.UpdateQuestionMultipleChoice(updateQuestionMultipleChoiceDTO);

            }
            else if (updateQuestionDTO is UpdateQuestionOpenEndedDTO updateQuestionOpenEndedDTO)
            {
                if (!(question is QuestionOpenEnded openEndedQuestion))
                {
                    throw new BadHttpRequestException("Question type mismatch");
                }

                openEndedQuestion.UpdateQuestionOpenEnded(updateQuestionOpenEndedDTO);
            }
            else
            {
                question.Text = updateQuestionDTO.Text;
                question.Order = updateQuestionDTO.Order;
                question.IsRequired = updateQuestionDTO.IsRequired;
            }
            question.IsDeleted = false;
            await _context.SaveChangesAsync();

            return question.ToQuestionDTO();

        }

        public async Task<QuestionDTO> DeleteQuestion(int id)
        {
            var question = await _context.Questions.Include(q => q.Form).Include(q => (q as QuestionMultipleChoice).Options).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                throw new EntityNotFoundException("Can't find question");
            }

            var userId = _userService.GetLoggedInUserId();
            if (userId == null)
            {
                throw new UnauthorizedException("User must be logged");
            }
            if (!question.Form.UserId.Equals(userId))
            {
                throw new UnauthorizedException("User not authorized to update question");
            }

            if (question.GetType().Name == "QuestionMultipleChoice")
            {
                ((QuestionMultipleChoice)question).DeleteQuestion();
            }
            else
            {
                question.DeleteQuestion();
            }
            _context.SaveChanges();
            return question.ToQuestionDTO();
        }

        // handling options

        public async Task<IEnumerable<OptionDTO>> GetOptionsByQuestion(int id)
        {
            var questionExists = await _context.Questions.FirstOrDefaultAsync(q => q.Id == id);
            if (questionExists == null)
            {
                throw new EntityNotFoundException("Can't find question");
            }
            var question = await _context.Questions.OfType<QuestionMultipleChoice>().Include(q => q.Options).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                throw new BadHttpRequestException("Question type mismatch. Only Multiple Choice Questions can have Options.");
            }

            return question.Options.Select(o => o.ToOptionDTO()).OrderBy(o => o.Order).ToList();
        }

        public async Task<QuestionDTO> AddOption(int id, CreateOptionDTO createOptionDTO)
        {
            var questionExists = await _context.Questions.FirstOrDefaultAsync(q => q.Id == id);
            if (questionExists == null)
            {
                throw new EntityNotFoundException("Can't find question");
            }

            var question = await _context.Questions.OfType<QuestionMultipleChoice>().Include(q => q.Options).Include(q => q.Form).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                throw new BadHttpRequestException("Question type mismatch. Only Multiple Choice Questions can have Options.");
            }
            var userId = _userService.GetLoggedInUserId();
            if (userId == null)
            {
                throw new UnauthorizedException("User must be logged");
            }
            if (!question.Form.UserId.Equals(userId))
            {
                throw new UnauthorizedException("User not authorized to update question");
            }
            var option = new Option(createOptionDTO);
            question.AddOption(option);
            await _context.SaveChangesAsync();
            return (QuestionMultipleChoiceDTO)question.ToQuestionDTO();
        }

        public async Task<QuestionDTO> DeleteOption(int questionId, int optionId)
        {
            var question = await _context.Questions.OfType<QuestionMultipleChoice>().Include(q => q.Options).Include(q => q.Form).FirstOrDefaultAsync(q => q.Id == questionId);
            if (question == null)
            {
                throw new EntityNotFoundException("Can't find question");
            }
            var option = question.Options.FirstOrDefault(o => o.Id == optionId);
            if (option == null)
            {
                throw new EntityNotFoundException("Can't find option");
            }
            var userId = _userService.GetLoggedInUserId();
            if (userId == null)
            {
                throw new UnauthorizedException("User must be logged");
            }
            if (!question.Form.UserId.Equals(userId))
            {
                throw new UnauthorizedException("User not authorized to update question");
            }

            option.DeleteOption();
            await _context.SaveChangesAsync();
            return (QuestionMultipleChoiceDTO)question.ToQuestionDTO();
        }
    }
}
