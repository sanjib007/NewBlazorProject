namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class ClientInformationForNewGoResponseModel
    {
        public ClientDatabaseMainResponseModel? clientDatabaseMainResponseModel { get; set; }
        public MisInstallationServicePackageInfo? misInstallationServicePackageInfo { get; set; }
        public CustomerBillingInfo? customerBillingInfo { get; set; }
    }
}
