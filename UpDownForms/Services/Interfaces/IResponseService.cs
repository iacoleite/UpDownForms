using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.ResponseDTOs;
using UpDownForms.Pagination;

namespace UpDownForms.Services.Interfaces
{
    public interface IResponseService
    {
        Task<Pageable<ResponseDTO>> GetResponses(PageParameters pageParameters);
        Task<ResponseFormNoResponseDTO> GetResponseById(int id);
        Task<ResponseDTO> PostResponse(CreateResponseDTO createResponseDTO);
        Task<ResponseDTO> DeleteResponse(int id);
        Task<AnswerDTO> PostAnswer(int id, CreateAnswerDTO createAnswerDTO);

    }
}
