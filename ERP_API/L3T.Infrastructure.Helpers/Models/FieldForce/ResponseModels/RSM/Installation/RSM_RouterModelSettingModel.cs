using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    public class RSM_RouterModelSettingModel
    {
        [Key]
        public int ID { get; set; }
        public string? CustomerID { get; set; }
        public int? RouterModelID { get; set; }
        public int? RouterModelSettingID { get; set; }
        public DateTime? RouterRebootTime { get; set; }
        public int? RouterRebootTimeID { get; set; }
        public int? RouterBrandID { get; set; }
        public string? EntryBy { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
