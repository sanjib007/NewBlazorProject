using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace L3T.Infrastructure.Helpers.Models.SystemSetting;

public class SystemSetting
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string ParamName { get; set; }
    public string ParamValue { get; set; }
    public DateTime? InsertedDateTime { get; set; }
}