namespace L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;

public class DeleteUserFromMikrotikRouerRequestModel : MikrotikRouterCommonModel
{
    public string CustomerIp { get; set; }
    public string? CallerId { get; set; }
}