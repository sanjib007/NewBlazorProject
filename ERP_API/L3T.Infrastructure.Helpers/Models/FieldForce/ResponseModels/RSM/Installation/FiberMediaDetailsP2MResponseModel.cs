using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    [Keyless]
    public class FiberMediaDetailsP2MResponseModel
    {
        public List<BtsResponseModel> BtsList { get; set; }

        [NotMapped]
        public int? selectedId { get; set; }
        public string? Splitter { get; set; }
        public string? OltBrand { get; set; }
        public string? OltName { get; set; }
        public string? Pon { get; set; }
        public string? Port { get; set; }
        public string? Laser { get; set; }
        public string? Portcapfb { get; set; }
        public string? Remarks { get; set; }
    }
}
