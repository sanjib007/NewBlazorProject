using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    public class Rsm_ClientNewAddressInfoResponseModel
    {
        [Key]
        public string MqID { get; set; }
        public string? DistrictName { get; set; }
        public string? UpazilaName { get; set; }
        public string? AreaName { get; set; }
        public string? PostCode { get; set; } = string.Empty;
        public string? SubscriberTitle { get; set; }
        public string? MariedName { get; set; }
        public string? HouseName { get; set; }
        //public string? HouseCode { get; set; }
        public string? GenderName { get; set; }
        public string? SupportOfficeName { get; set; }
        public string? IDProofName { get; set; }
        public string? Nationality { get; set; }
        public string? OccupationName { get; set; }
        public string? FlatNo { get; set; }
        public string? RoadNo { get; set; }
        public string? section { get; set; }
        public string? landmark { get; set; }
        public string? houseno { get; set; }
        public string? RoadName_No { get; set; }
        public string? Block { get; set; }
        public string? Sector { get; set; }

    }
}
