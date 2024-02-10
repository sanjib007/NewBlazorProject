namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class NetworkUpdateInfoResponseModel
    {
        public ClientDatabaseMainResponseModel? ClientDatabaseMainModel { get; set; }
        public ClientTechnicalInfoResponseModel? ClientTechnicalInfoModel { get; set; }
        public List<ClientDatabaseEquipmentResponseModel>? ClientDatabaseEquipmentListModel { get; set; }
        public List<ClientDatabaseItemDetResponseModel>? ClientDatabaseItemDetListModel { get; set; }
        public tbl_SubSpliterEntryResponseModel? tbl_SubSpliterEntryModel { get; set; }
        public List<tbl_darkfiber_clientResponseModel>? tbl_Darkfiber_ClientListModel { get; set; }
        public tbl_NttnDetailsResponseModel? tbl_NttnDetailsModel { get; set; }
    }
}
