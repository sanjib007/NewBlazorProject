using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class Cli_PendingResponseModel
    {
        public string? RefNo { get; set; }
        public string? Service { get; set; }
        public string? Status { get; set; }
        public string? userid { get; set; }
        public string? Mkt_group { get; set; }
        [Key]
        public int? SLNo { get; set; }
        public int? PrioritySet { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinisDate { get; set; }
    }
}
