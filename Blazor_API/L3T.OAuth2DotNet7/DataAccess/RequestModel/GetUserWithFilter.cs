using L3T.OAuth2DotNet7.Pagination.Filter;

namespace L3T.OAuth2DotNet7.DataAccess.RequestModel;

public class GetUserWithFilter : PaginationFilter
{
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? L3TID { get; set; }
    public string? Email { get; set; }
    public bool? EmailConfirm { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? PhoneNumberConfirm { get; set; }
    public bool? IsActive { get; set; }
}