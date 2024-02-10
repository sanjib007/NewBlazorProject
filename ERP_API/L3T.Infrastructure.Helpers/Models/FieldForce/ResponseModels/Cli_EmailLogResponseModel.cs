using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{

    public class Cli_EmailLogResponseModel
    {
        public string? CTID { get; set; }
        public string? Mailfrom { get; set; }
        public string? Mailto { get; set; }
        public string? MailCC { get; set; }
        public string? MailBcc { get; set; }
        public string? MailCategory { get; set; }
        public string? MailSubject { get; set; }
        public string? MailBody { get; set; }
        public DateTime? MailSentTime { get; set; }
        public string? Status { get; set; }
        [Key]
        public int id { get; set; }
    }
}
