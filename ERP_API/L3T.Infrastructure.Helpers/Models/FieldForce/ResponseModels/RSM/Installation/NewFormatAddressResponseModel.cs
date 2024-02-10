using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    public class NewFormatAddressResponseModel
    {
        [Key]
        public string? DistrictName { get; set; }
        public string? PostCode { get; set; }
        public string? HouseName { get; set; }
        public string? FlatNo { get; set; }
        public string? RoadNo { get; set; }
        public string? section { get; set; }
        public string? landmark { get; set; }
        public string? UpazilaName { get; set; }
        public string? AreaName { get; set; }
        public string? houseno { get; set; }
        public string? RoadName_No { get; set; }
    }
}
