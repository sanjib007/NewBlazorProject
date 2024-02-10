using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.BTS.BtsDTO
{
    //public class BtsInfoDTO:BtsInfo
    //{
    //}
    public class BtsInfoDTO 
    {
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
    }
}
