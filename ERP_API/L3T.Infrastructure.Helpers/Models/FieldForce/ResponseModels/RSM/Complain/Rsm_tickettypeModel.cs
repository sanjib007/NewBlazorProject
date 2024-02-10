using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain
{
    public class Rsm_tickettypeModel
    {
        [Key]
        public int ID { get; set; }
        public string? TicketTypeName { get; set; }
    }
}
