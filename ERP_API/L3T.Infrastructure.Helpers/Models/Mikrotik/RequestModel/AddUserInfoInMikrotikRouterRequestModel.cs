namespace L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;

public class AddUserInfoInMikrotikRouterRequestModel : MikrotikRouterCommonModel
{
    public string LBLpackageplan { get; set; }
    public string TXTRealIP { get; set; }
    public string MySubId { get; set; }
    public string? CallerId { get; set; }
}