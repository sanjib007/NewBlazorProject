namespace Cr.UI.Data.Socket
{
    public class GetUserInfoFromMikrotikRequestModel  :MikrotikRouterCommonModel
    {
        public string CustomerID { get; set; }
        public string? CallerId { get; set; }
    }
}
