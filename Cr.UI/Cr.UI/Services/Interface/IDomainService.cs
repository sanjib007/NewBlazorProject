using Cr.UI.Data;

namespace Cr.UI.Services.Interface
{
    public interface IDomainService
    {
        Task<ApiResponse<PaginationModel<List<GuestModel>>>> GetAllAsync(string requestUri);
        Task<ApiResponse> GetByIdAsync(string requestUri, int Id);
        Task<ApiResponse> SaveAsync(string requestUri, GuestModel obj);
        Task<ApiResponse> UpdateSaveAsync(string requestUri, GuestModel obj);
        Task<ApiResponse> DeleteAsync(string requestUri);
    }
}
