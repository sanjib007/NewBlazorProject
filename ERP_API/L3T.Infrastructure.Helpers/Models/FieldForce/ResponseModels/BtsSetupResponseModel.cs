using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class BtsSetupResponseModel
    {
        [Key]
        public int BtsSetupID { get; set; }
        public string? BtsSetupName { get; set; }
        //public string NewBtsName { get; set; }
        // public int NewBtsID { get; set; }
        public string? BtsName { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? EntryUserID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifyingUserID { get; set; }
        public int? DivisionID { get; set; }
        //public int DistrictID { get; set; }
        //public int ThanaID { get; set; }
        public int? AreaID { get; set; }
        public string? Area { get; set; }
        //public int SupportOfficeID { get; set; }
        //public int TypeID { get; set; }
        public int? BTSStatus { get; set; }
        //public string? Latitude { get; set; }
        //public string? Longitude { get; set; }
        //public string? Zone { get; set; }
        //public string? BtsAddress { get; set; }
        //public string? BtsMode { get; set; }
        //public string? BuildingHeight { get; set; }
        //public string? TowerHeight { get; set; }
        //public string? TowerType { get; set; }
        //public string? UPS { get; set; }
        //public string? UPSBackupTime { get; set; }
        //public string? Generator { get; set; }
    }
}
