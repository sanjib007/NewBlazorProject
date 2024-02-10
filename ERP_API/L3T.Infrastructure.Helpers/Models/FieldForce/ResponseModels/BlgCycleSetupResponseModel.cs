using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class BlgCycleSetupResponseModel
    {
        [Key]
        public int BillingCycleID { get; set; }
        public int BillingCycleValue { get; set; }
        public string BillingCycleName { get; set; }
    }
}
