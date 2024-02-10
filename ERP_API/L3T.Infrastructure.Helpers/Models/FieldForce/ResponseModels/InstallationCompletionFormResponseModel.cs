namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class InstallationCompletionFormResponseModel
    {
        public ClientDatabaseMainResponseModel? ClientDatabaseMainModel { get; set; }
        public List<ClientDatabaseItemDetResponseModel>? ClientDatabaseItemDetListModel { get; set; }
        public InstallationCompletionMediaInfo? InstallationCompletionMediaInfoModel { get; set; }
        public ClientTechnicalInfoResponseModel? ClientTechnicalInfoModel { get; set; }
        public List<ClientDatabaseEquipmentResponseModel>? ClientDatabaseEquipmentListModel { get; set; }
    }
}
