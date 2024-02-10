using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class IntranetInfoResponseModel
    {

        [NotMapped]
        public List<BtsSetupListResponseModel>? BtsSetupList { get; set; }

        [NotMapped]
        public int? BtsSelectedId { get; set; }
        public string? LastmileBandwidth { get; set; }
        public string? IntercityBandwidth { get; set; }
        public string? private_ip { get; set; }
        public string? private_gateway { get; set; }
        public string? private_musk { get; set; }
        public string? Pvlan { get; set; }
        public string? mrtg_link { get; set; }
    }
}
