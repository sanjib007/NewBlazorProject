using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.SystemErrorLog;

public class SystemErrorLog
{
    [Key]
    public long Id { get; set; }
    public string MethodName { get; set; }
    public string ErrorMessage { get; set; }
    public string ErrorDescription { get; set; }
    public DateTime InsertedDate { get; set; }
}