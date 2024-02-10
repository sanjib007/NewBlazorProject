using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    public class UpdateHistroryResponseModel
    {
        [Key]
        public int ID { get; set; }
        public string? UpdateUser { get; set; }
        public string? DesgAndDeptAndSectAndCell { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Remarks { get; set; }    
    }
}
