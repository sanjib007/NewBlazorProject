using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.SmsNotification;

public class SmsNotifyRequestResponse
{
    [Key]
    public long SmsNotifyRequestResponseId { get; set; }
    public string MethordName { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}