using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.Mikrotik;

public class MikrotikRequestResponse
{
    [Key]
    public long MikrotikRequestResponseId { get; set; }
    public string RouterIp { get; set; }
    public string CustomerIp { get; set; }
    public string MethordName { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }
    public string UserId { get; set; }
    public string SubId { get; set; }
    public DateTime CreatedAt { get; set; }
}