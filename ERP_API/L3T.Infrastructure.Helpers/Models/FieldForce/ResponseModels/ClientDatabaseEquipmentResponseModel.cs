using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{

    public class ClientDatabaseEquipmentResponseModel
    {
        public string? cli_code { get; set; }
        public int? cli_sl_no { get; set; }
        public string? equipments { get; set; }
        public string? description { get; set; }
        public string? slno { get; set; }
        public string? quantity { get; set; }
        public string? ownership { get; set; }
        public string? TechName { get; set; }
        public string? HandoverDate { get; set; }
        public string? CoordinatorName { get; set; }
        public string? Throuputtest { get; set; }
        public string? Tag { get; set; }
        [Key]
        public int unique_id { get; set; }
    }
}
