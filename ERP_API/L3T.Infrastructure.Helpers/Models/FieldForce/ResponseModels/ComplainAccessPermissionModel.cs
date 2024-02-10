using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class ComplainAccessPermissionModel
    {
        [Key]
        public string? ComplainID { get; set; }
        public DateTime? AccessPermissionDatetime { get; set; }
        public string? Comments { get; set; }        
        public DateTime ComplainRecordDatetime { get; set; }
        public DateTime? LinkDownAt { get; set; }
        public DateTime? ResulationTime { get; set; }       
    }
}
