using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    [Keyless]
    public class ConnectivityAddressResponseModel
    {
        public string? HouseName { get; set; }
        public string? Area { get; set; }
        public string? RoadName { get; set; }
        public string? Sector { get; set; }
        public string? Section { get; set; }
        public string? Block { get; set; }
        public string? Upazila { get; set; }
        public string? PoliceStation	 { get; set; }
        public string? PostCode { get; set; }
        public string? District { get; set; }
        public string? Services { get; set; }
        public List<TeamResponseModel> TeamList { get; set; }

    }
}
