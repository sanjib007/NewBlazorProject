using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class MailLogResponseModel
    {
        public string? MailBody { get; set; }
    }
}
