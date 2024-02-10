using Cr.UI.Data;

namespace Cr.UI.Services.Interface
{
    public interface IGenericService<T>
    {
        Task<ApiResponse<List<T>>> GetAllAsync(string requestUri);
        Task<ApiResponse> GetWithoutResponse(string requestUri);
        Task<ApiResponse<T>> GetOnlyAsync(string requestUri);
        Task<ApiResponse<T>> GetByIdAsync(string requestUri, int Id);
        Task<ApiResponse<T>> SaveAsync(string requestUri, T obj);
        Task<ApiResponse<T>> UpdateAsync(string requestUri, int Id, T obj);
        Task<ApiResponse> DeleteAsync(string requestUri, int Id);
    }
}
