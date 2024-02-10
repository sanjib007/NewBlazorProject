using L3T.OAuth2DotNet7.Pagination.Filter;
using Microsoft.AspNetCore.WebUtilities;

namespace L3T.OAuth2DotNet7.Pagination;

public interface IUriService
{
    public Uri GetPageUri(PaginationFilter filter, string route);
}
public class UriService : IUriService
{
    private readonly string _baseUri;
    public UriService(string baseUri)
    {
        _baseUri = baseUri;
    }
    public Uri GetPageUri(PaginationFilter filter, string route)
    {
        var _enpointUri = new Uri(string.Concat(_baseUri, route));
        var modifiedUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
        modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
        return new Uri(modifiedUri);
    }
}
