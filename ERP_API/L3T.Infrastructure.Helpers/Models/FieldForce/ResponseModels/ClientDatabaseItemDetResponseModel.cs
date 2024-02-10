using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class ClientDatabaseItemDetResponseModel
    {
        public string brCliCode { get; set; }
        public int brSlNo { get; set; }
        public string itm_type { get; set; }
        public int item_id { get; set; }
        public string item_desc { get; set; }
        public int ServiceStatus { get; set; }
        [Key]
        public int Slno { get; set; }
    }
}
