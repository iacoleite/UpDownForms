using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using System.Net;
using UpDownForms.DTO.ApiResponse;
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

        public async Task<ApiResponse<IEnumerable<QuestionDetailsDTO>>> GetQuestions()
        {
            var response = await _context.Questions.Include(q => q.Form).Where(q => !q.IsDeleted).ToListAsync();
            return new ApiResponse<IEnumerable<QuestionDetailsDTO>>(true, "OK", response.Select(q => q.ToQuestionDetailsDTO()).ToList());
        }

        public async Task<ApiResponse<QuestionDTO>> GetQuestionById(int id)
        {
            var question = await _context.Questions.Include(q => q.Form).Include(q => q.Answers).Include(q => (q as QuestionMultipleChoice).Options).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return new ApiResponse<QuestionDTO>(false, "Can't find question", null);
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

            return new ApiResponse<QuestionDTO>(true, "ok", questionDTO);
        }

        public async Task<ApiResponse<QuestionDetailsDTO>> PostQuestion(CreateQuestionDTO createQuestionDTO)
        {
            if (createQuestionDTO == null)
            {
                return new ApiResponse<QuestionDetailsDTO>(false, "Missing input data", null);
            }

            var form = await _context.Forms.FindAsync(createQuestionDTO.FormId);
            if (form == null)
            {
                
                return new ApiResponse<QuestionDetailsDTO>(false, "Can't find form to add question", null);
            }

            var userId = _userService.GetLoggedInUserId();

            if (!form.UserId.Equals(userId))
            {
                return new ApiResponse<QuestionDetailsDTO>(false, "Logged user does not authorization to post to form", null);
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
                return new ApiResponse<QuestionDetailsDTO>(false, "Missing question data", null);

            }
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return new ApiResponse<QuestionDetailsDTO>(true, "OK", question.ToQuestionDetailsDTO());
        }

        public async Task<ApiResponse<QuestionDTO>> PutQuestion(int id, UpdateQuestionDTO updateQuestionDTO)
        {
            if (updateQuestionDTO == null)
            {
                return new ApiResponse<QuestionDTO>(false, "Missing question data", null);

            }
            var question = await _context.Questions.Include(q => q.Form).Include(q => (q as QuestionMultipleChoice).Options).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return new ApiResponse<QuestionDTO>(false, "Can't find question", null);
            }
            var userId = _userService.GetLoggedInUserId();
            var formId = question.Form.UserId;
            if (!formId.Equals(userId))
            {
                return new ApiResponse<QuestionDTO>(false, "Logged user does not authorization to update question", null);
            }

            if (updateQuestionDTO is UpdateQuestionMultipleChoiceDTO updateQuestionMultipleChoiceDTO)
            {
                if (!(question is QuestionMultipleChoice multipleChoiceQuestion))
                {
                    return new ApiResponse<QuestionDTO>(false, "Question type mismatch", null);
                }
                multipleChoiceQuestion.Text = updateQuestionMultipleChoiceDTO.Text;
                multipleChoiceQuestion.Order = updateQuestionMultipleChoiceDTO.Order;
                multipleChoiceQuestion.IsRequired = updateQuestionMultipleChoiceDTO.IsRequired;
                multipleChoiceQuestion.HasCorrectAnswer = updateQuestionMultipleChoiceDTO.HasCorrectAnswer;
                //multipleChoiceQuestion.QuestionType = updateQuestionMultipleChoiceDTO.Type;
                //multipleChoiceQuestion.IsDeleted = false;
                multipleChoiceQuestion.UndeleteQuestionAndOptions();

            }
            else if (updateQuestionDTO is UpdateQuestionOpenEndedDTO updateQuestionOpenEndedDTO)
            {
                if (!(question is QuestionOpenEnded openEndedQuestion))
                {
                    return new ApiResponse<QuestionDTO>(false, "Question type mismatch", null);
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
            
            return new ApiResponse<QuestionDTO>(true, "OK", question.ToQuestionDTO());

        }

        public async Task<ApiResponse<QuestionDTO>> DeleteQuestion(int id)
        {
            var question = await _context.Questions.Include(q => q.Form).Include(q => (q as QuestionMultipleChoice).Options).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return new ApiResponse<QuestionDTO>(false, "Question not found", null);
            }

            var userId = _userService.GetLoggedInUserId();
            var formId = question.Form.UserId;
            if (!formId.Equals(userId))
            {
                return new ApiResponse<QuestionDTO>(false, "Logged user does not authorization to update question", null);                
            }

            if (question.GetType().Name == "QuestionMultipleChoice") 
            {
                ((QuestionMultipleChoice)question).DeleteQuestion();
            }
            else
            {
                question.DeleteQuestion();
            }
            //question.DeleteQuestion();
            _context.SaveChanges();
            return new ApiResponse<QuestionDTO>(true, "Question Deleted", question.ToQuestionDTO());
        }

        // handling options

        public async Task<ApiResponse<IEnumerable<OptionDTO>>> GetOptionsByQuestion(int id)
        {
            var question = await _context.Questions.OfType<QuestionMultipleChoice>().Include(q => q.Options).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return new ApiResponse<IEnumerable<OptionDTO>>(false, "Can't find question", null);
            }

            return new ApiResponse<IEnumerable<OptionDTO>>(true, "ok", question.Options.Select(o => o.ToOptionDTO()).ToList());
        }

        public async Task<ApiResponse<QuestionDTO>> AddOption(int id, CreateOptionDTO createOptionDTO)
        {
            var question = await _context.Questions.OfType<QuestionMultipleChoice>().Include(q => q.Options).Include(q => q.Form).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return new ApiResponse<QuestionDTO>(false, "Question not found", null);
            }
            var userId = _userService.GetLoggedInUserId();
            var formId = question.Form.UserId;
            if (!formId.Equals(userId))
            {
                return new ApiResponse<QuestionDTO>(false, "Logged user does not authorization to add options to question", null);
            }
            var option = new Option(createOptionDTO);
            question.AddOption(option);
            await _context.SaveChangesAsync();
            return new ApiResponse<QuestionDTO>(true, "Option added to question", (QuestionMultipleChoiceDTO)question.ToQuestionDTO());
        }

        public async Task<ApiResponse<QuestionDTO>> DeleteOption(int questionId, int optionId)
        {
            var question = await _context.Questions.OfType<QuestionMultipleChoice>().Include(q => q.Options).Include(q => q.Form).FirstOrDefaultAsync(q => q.Id == questionId);
            if (question == null)
            {
                return new ApiResponse<QuestionDTO>(false, "Question not found", null);
            }
            var option = question.Options.FirstOrDefault(o => o.Id == optionId);
            if (option == null)
            {
                return new ApiResponse<QuestionDTO>(false, "Option not found", null);
            }
            var userId = _userService.GetLoggedInUserId();
            var formId = question.Form.UserId;
            if (!formId.Equals(userId))
            {
                return new ApiResponse<QuestionDTO>(false, "Logged user does not authorization to Delete options", null);
            }

            option.DeleteOption();
            await _context.SaveChangesAsync();
            return new ApiResponse<QuestionDTO>(true, "Option DELETED", (QuestionMultipleChoiceDTO)question.ToQuestionDTO());
        }
    }
}
