using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class WireSetupModel
    {
        [Key]
        public int WireID { get; set; }
        public string? WireName { get; set; }
    }
}
