using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class MisInstallationServicePackageInfo
    {
        public int DisPackageID { get; set; }
        public int PackageID { get; set; }
        public string DistributorID { get; set; }
        public string DistributorName { get; set; }
        public string PackagePlan { get; set; }
        public string UpdatePackageName { get; set; }
        public string DistributorSubscriberID { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceNarration { get; set; }
        public decimal OTCAmount { get; set; }
        public decimal Amount { get; set; }
        public string SubscriberPassword { get; set; }
    }
}
