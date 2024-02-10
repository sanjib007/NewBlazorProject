using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;

public class BlockUserFromMikrotikRouterRequestModel  : MikrotikRouterCommonModel
{
    public string CustomerIp { get; set; }
    public string? CallerId { get; set; }
}