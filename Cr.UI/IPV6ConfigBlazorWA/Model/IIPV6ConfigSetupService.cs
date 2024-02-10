namespace IPV6ConfigBlazorWA.Model
{
    public interface IIPV6ConfigSetupService<T>
    {
        Task<ApiResponse<List<T>>> GetAllAsync(string requestUri);
        Task<ApiResponse<T>> SaveAsync(string requestUri, T obj);
    }
}
