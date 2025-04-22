using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Dynamic.Core;
using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.OptionDTOs;
using UpDownForms.DTO.QuestionDTOs;
using UpDownForms.Models;
using UpDownForms.Pagination;
using System.Collections.Immutable;
using UpDownForms.Services.Interfaces;

namespace UpDownForms.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUpDownFormsContext _context;
        private readonly ILoggedUserService _userService;

        public QuestionService(IUpDownFormsContext context, ILoggedUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Pageable<QuestionDetailsDTO>> GetQuestions(PageParameters pageParameters)
        {
            var orderParam = PageParamValidator.SetSortOrder<AnswerDTO>(pageParameters);

            var response = _context.Questions.Include(q => q.Form).Where(q => !q.IsDeleted).OrderBy(orderParam).Select(q => q.ToQuestionDetailsDTO());
            if (response == null)
            {
                throw new EntityNotFoundException();
            }
            var pageable = await Pageable<QuestionDetailsDTO>.ToPageable(response, pageParameters);
            //return response.Select(q => q.ToQuestionDetailsDTO()).ToList();
            if (pageable.Items.Count() == 0)
            {
                throw new EntityNotFoundException();
            }
            return pageable;
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

            var isAuthorized = await _userService.IsAuthorized(form);

            if (!isAuthorized)
            {
                throw new UnauthorizedException("You are not authorized to post in this form");
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

            var isAuthorized = await _userService.IsAuthorized(question);

            if (!isAuthorized)
            {
                throw new UnauthorizedException("You are not authorized to update this question");
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

            var isAuthorized = await _userService.IsAuthorized(question);

            if (!isAuthorized)
            {
                throw new UnauthorizedException("You are not authorized to delete this question");
            }


            if (question.GetType().Name == "QuestionMultipleChoice")
            {
                ((QuestionMultipleChoice)question).DeleteQuestion();
            }
            else
            {
                question.DeleteQuestion();
            }
            await _context.SaveChangesAsync();
            return question.ToQuestionDTO();
        }

        // handling options

        public async Task<Pageable<OptionDTO>> GetOptionsByQuestion(int id, PageParameters pageParameters)
        {
            var questionExists = _context.Questions.FirstOrDefault(q => q.Id == id);
            if (questionExists == null)
            {
                throw new EntityNotFoundException("Can't find question");
            }
            var question = _context.Questions.OfType<QuestionMultipleChoice>().Include(q => q.Options).FirstOrDefault(q => q.Id == id);
            if (question == null)
            {
                throw new BadHttpRequestException("Question type mismatch. Only Multiple Choice Questions can have Options.");
            }

            var orderParam = PageParamValidator.SetSortOrder<OptionDTO>(pageParameters);

            var options = _context.Options
                                  .Where(o => o.QuestionId == id)
                                  .OrderBy(orderParam)
                                  .Select(o => o.ToOptionDTO());
            var pageable = await Pageable<OptionDTO>.ToPageable(options, pageParameters);
            if (pageable.Items.Count() == 0)
            {
                throw new EntityNotFoundException();
            }
            return pageable;
        }

        public async Task<QuestionDTO> AddOption(int id, CreateOptionDTO createOptionDTO)
        {
            var questions = _context.Questions.Where(q => q.Id == id);
            if (!questions.Any())
                throw new EntityNotFoundException("Can't find question");
                
            var question = await questions.OfType<QuestionMultipleChoice>().Include(q => q.Options).Include(q => q.Form).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                throw new BadHttpRequestException("Question type mismatch. Only Multiple Choice Questions can have Options.");
            }
            var isAuthorized = await _userService.IsAuthorized(question);

            if (!isAuthorized)
            {
                throw new UnauthorizedException("You are not authorized to add options to this question");
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
            var isAuthorized = await _userService.IsAuthorized(question);

            if (!isAuthorized)
            {
                throw new UnauthorizedException("You are not authorized to delete this option");
            }

            option.DeleteOption();
            await _context.SaveChangesAsync();
            return (QuestionMultipleChoiceDTO)question.ToQuestionDTO();
        }

        public async Task<Pageable<AnswerDTO>> GetAllAnswersByQuestionId(int questionId, PageParameters pageParameters)
        {
            var question = _context.Questions.Include(q => q.Form).FirstOrDefault(q => q.Id == questionId);
            if (question == null)
            {
                throw new EntityNotFoundException("Can't find question");
            }

            var isAuthorized = await _userService.IsAuthorized(question);

            if (!isAuthorized)
            {
                throw new UnauthorizedException("You are not authorized to access the answers of this question");
            }
            var orderParam = PageParamValidator.SetSortOrder<AnswerDTO>(pageParameters);

            var response = _context.Answers.Where(a => a.QuestionId == questionId).OrderBy(orderParam).Select(a => a.ToAnswerDTO());
            var pageable = await Pageable<AnswerDTO>.ToPageable(response, pageParameters);
            if (pageable.Items.Count() == 0)
            {
                throw new EntityNotFoundException("Question does not have any answer.");
            }
            return pageable;
        }
    }
}
