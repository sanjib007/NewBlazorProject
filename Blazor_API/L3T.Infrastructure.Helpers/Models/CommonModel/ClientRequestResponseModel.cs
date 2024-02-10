using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.CommonModel
{
    public class ClientRequestResponseModel
    {
        [Key]
        public long Id { get; set; }
        public string? Request { get; set; }
        public string? Response { get; set; }
        public string? UserId { get; set; }
        public string? ErrorLog { get; set; }
        public string? RequestedIP { get; set; }
        public string? MethodName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
       
        
    }
}