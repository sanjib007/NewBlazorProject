using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class tbl_NttnDetailsResponseModel
    {
        [Key]
        public int AutoID { get; set; }
        public string? SubscriberID { get; set; }
        public int? SlNO { get; set; }
        public int? CableNetworkID { get; set; }
        public int? NTTNNameID { get; set; }
        public int? Typeofp2mlinkID { get; set; }
        public string? CoreName { get; set; }
        public string? SCR_LinkID { get; set; }
        public string? SummitLinkID { get; set; }
        public string? BahonCoreID { get; set; }
        public string? Remarks { get; set; }
    }
}
