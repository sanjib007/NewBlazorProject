using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    [Keyless]
    public class ConnectivityDetailsResponseModel
    {
        public List<CableNoResponseMode> CableNo { get; set; }
        public string? Color { get; set; }
        public string? ShardOnuMac { get; set; }
        public string? ShardOnuPort { get; set; }
        public bool? IsSharedVisible { get; set; }
    }
}
