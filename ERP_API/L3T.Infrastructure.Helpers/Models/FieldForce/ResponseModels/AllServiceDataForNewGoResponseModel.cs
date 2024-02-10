namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class AllServiceDataForNewGoResponseModel
    {
        public Post_MainInsResponseModel? Post_MainIns { get; set; }
        public List<ClientDatabaseItemDetResponseModel>? ClientDatabaseItemDetList { get; set; }
        public tbl_ODFNameSetUpResponseModel? tbl_ODFNameSetUp { get; set; }
        public ClientTechnicalInfoResponseModel? ClientTechnicalInfo { get; set; }
        public List<ClientDatabaseMediaDetailsResponseModel>? ClientDatabaseMediaDetailsList { get; set; }
    }
}
