using L3TIdentityOAuth2Server.Pagination.Filter;

namespace L3TIdentityOAuth2Server.DataAccess.RequestModel;

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