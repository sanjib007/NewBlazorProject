using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class ClientDatabaseP2PAddressResponseModel
    {
        public string? brCliCode { get; set; }
        public int? brSlNo { get; set; }
        public string? brAdrCode { get; set; }
        public string? brAdrNewCode { get; set; }
        public string? brName { get; set; }
        public string? brAddress1 { get; set; }
        public string? brAddress2 { get; set; }
        public int? brAreaGroupId { get; set; }
        public string? brAreaGroup { get; set; }
        public int? brAreaId { get; set; }
        public string? brArea { get; set; }
        public string? contact_det { get; set; }
        public string? Contact_Designation { get; set; }
        public string? phone_no { get; set; }
        public string? fax_no { get; set; }
        public string? email_id { get; set; }
        public string? add_for_p2p { get; set; }
        [Key]
        public int sll { get; set; }
    }
}
