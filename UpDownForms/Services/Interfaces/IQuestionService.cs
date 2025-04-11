using Microsoft.EntityFrameworkCore.Storage;
using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.OptionDTOs;
using UpDownForms.DTO.QuestionDTOs;
using UpDownForms.Pagination;

namespace UpDownForms.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<Pageable<QuestionDetailsDTO>> GetQuestions(PageParameters pageParameters);
        Task<QuestionDTO> GetQuestionById(int id);
        Task<QuestionDetailsDTO> PostQuestion(CreateQuestionDTO createQuestionDTO);
        Task<QuestionDTO> PutQuestion(int id, UpdateQuestionDTO updateQuestionDTO);
        Task<QuestionDTO> DeleteQuestion(int id);
        Task<Pageable<OptionDTO>> GetOptionsByQuestion(int id, PageParameters pageParameters);
        Task<QuestionDTO> AddOption(int id, CreateOptionDTO createOptionDTO);
        Task<QuestionDTO> DeleteOption(int questionId, int optionId);
        Task<Pageable<AnswerDTO>> GetAllAnswersByQuestionId(int questionId, PageParameters pageParameters);


    }
}
