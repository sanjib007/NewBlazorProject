using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class IpTvPackageInfoResponseModel
    {
        public string? NoteBandwith { get; set; }
        public string? ServiceCode { get; set; }
        public decimal? Amount { get; set; }
    }
}
