namespace L3T.Infrastructure.Helpers.Pagination.Wreppers;

public class Response<T>
{
    public Response()
    {
    }
    public Response(T data)
    {
        Item = data;
    }
    public T Item { get; set; }
}