using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class tbl_WebServerResponseModel
    {
        [Key]
        public int SID { get; set; }
        public string? ServerID { get; set; }
        public string? ServerIP { get; set; }
        public string? ServiceProvider { get; set; }
        public string? ServerLocation { get; set; }
        public string? PurchaseSpace { get; set; }
        public decimal? HostingCost { get; set; }
    }
}
