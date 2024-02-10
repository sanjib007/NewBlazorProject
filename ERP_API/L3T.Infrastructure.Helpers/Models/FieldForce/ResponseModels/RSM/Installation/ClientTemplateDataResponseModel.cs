namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    public class ClientTemplateDataResponseModel
    {
        public Rsm_ClientDatabaseItemDetailsResponseModel? ClientDatabaseItemDetailsModel { get; set; }
        public Rsm_ClientTechnicalInfoResponseModel? ClientTechnicalInfoModel { get; set; }
        public Rsm_tbl_SubSpliterEntryResponseModel? SubSpliterEntryModel { get; set; }
        public Rsm_tbl_team_mem_permissionResponseModel? Team_Mem_PermissionsModel { get; set; }
        public RSM_SharedCustomerDetailsResponseModel? SharedCustomerDetailsModel { get; set; }
        public Rsm_ClientNewAddressInfoResponseModel? ClientNewAddressInfoModel { get; set; }

    }
}
