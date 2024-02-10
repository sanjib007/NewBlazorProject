using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    public class Rsm_EntryOperatorInfoOfTicketResponseModel
    {
        [Key]
        public int ID { get; set; }
        public string? Update_User { get; set; }
        public string? Desg_Dept_Sect_Cell { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Remarks { get; set; }
    }
}
