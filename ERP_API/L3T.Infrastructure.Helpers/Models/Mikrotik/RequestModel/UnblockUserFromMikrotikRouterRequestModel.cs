namespace L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;

public class UnblockUserFromMikrotikRouterRequestModel : MikrotikRouterCommonModel
{
    public string CustomerIp { get; set; }
    public string? CallerId { get; set; }
}