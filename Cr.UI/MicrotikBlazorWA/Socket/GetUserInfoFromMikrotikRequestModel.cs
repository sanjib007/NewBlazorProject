namespace MicrotikBlazorWA.Socket
{
    public class GetUserInfoFromMikrotikRequestModel : MikrotikRouterCommonModel
    {
        public string CustomerIp { get; set; }
        public string? CallerId { get; set; }
    }
}
