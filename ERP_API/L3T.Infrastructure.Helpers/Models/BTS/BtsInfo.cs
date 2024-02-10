using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.BTS
{
    public class BtsInfo
    {
        [Key]
        public long Id { get; set; }
        //public string BtsCode { get; set; }
        public int ZoneId { get; set; }
        public int DivisionId { get; set; }
        public int DistrictId { get; set; }
        public int ThanaId { get; set; }
        public string Area { get; set; }
        public long SupportOfficeId { get; set; }
        public long BTSTypeId { get; set; }
        public string OldBTSName { get; set; }
        public string BTSName { get; set; }
        public bool Status { get; set; }
        public long BTSModeId { get; set; }
        public string BuildingHeight { get; set; }
        public string TowerHeight { get; set; }
        public long TowerTypeId { get; set; }        
        public long Latitude { get; set; }
        public long Longitude { get; set; }
        public long UPSBackupId { get; set; }        
        public string UPS { get; set; }
        public string Generator { get; set; }
        public string? HouseName { get; set; }
        public string? HouseNo { get; set; }
        public string? FlatNo { get; set; }
        public string? RoadName { get; set; }
        public string? RoadNo { get; set; }
        public string? BlockNo { get; set; }
        public string? Sector { get; set; }
        public string? LandMark { get; set; }
        public string? PostalCode { get; set; }


        #region Audit field
        public string InsertedBy { get; set; }
        public DateTime InsertedDateTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        #endregion

        #region Navigation Property

        [ForeignKey("SupportOfficeId")]
        //[InverseProperty("BtsInfo")]
        public virtual SupportOffice SupportOffice { get; set; } = null!;

        [ForeignKey("BTSTypeId")]
        //[InverseProperty("BtsInfo")]
        public virtual BTSType BTSType { get; set; } = null!;

        [ForeignKey("BTSModeId")]
        //[InverseProperty("BtsInfo")]
        public virtual BTSMode BTSMode { get; set; } = null!;

        [ForeignKey("TowerTypeId")]
        //[InverseProperty("BtsInfo")]
        public virtual TowerType TowerType { get; set; } = null!;

        [ForeignKey("UPSBackupId")]
        //[InverseProperty("BtsInfo")]
        public virtual UPSBackup UPSBackup { get; set; } = null!;

        //[InverseProperty("BtsInfo")]
        //public ICollection<Router> Routers { get; set; }
        
        #endregion
    }
}
