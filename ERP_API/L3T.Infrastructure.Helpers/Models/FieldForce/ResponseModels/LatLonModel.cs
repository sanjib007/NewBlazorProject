using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class LatLonModel
    {
        public long Id { get; set; }
        public string Emp_id { get; set; }
        public string Device_Id { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string? Address { get; set; }
        public int? Sas_id { get; set; }
        public DateTime Date_added { get; set; }
    }
}
