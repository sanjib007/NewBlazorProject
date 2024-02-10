using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    public class ViewColorResponseModel
    {
        [Key]
        public int? autoid { get; set; }
        public string? StartPoint { get; set; }
        public string? CableType { get; set; }
        public int? TubeColor { get; set; }
        public string? TubeColorName { get; set; }
        public int? CoreColor { get; set; }
        public string? CoreColorName { get; set; }
        public string? CableID { get; set; }
        public string? StartMeter { get; set; }
        public string? EndMeter { get; set; }
        public string? EndPoint { get; set; }
        public string? Remarks { get; set; }
        public int? Position { get; set; }
    }
}
