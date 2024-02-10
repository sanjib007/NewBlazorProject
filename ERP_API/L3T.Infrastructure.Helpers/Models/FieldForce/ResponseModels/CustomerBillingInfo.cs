using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class CustomerBillingInfo
    {
        public string brCliCode { get; set; }
        public string i_ins_ref_no { get; set; }
        public decimal MRC { get; set; }
        public decimal OTC { get; set; }
    }
}
