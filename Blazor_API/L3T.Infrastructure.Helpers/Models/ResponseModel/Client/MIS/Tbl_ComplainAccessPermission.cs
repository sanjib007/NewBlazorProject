using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel.Client.MIS
{
    
    public class Tbl_ComplainAccessPermission
    {
        public string? ComplainID { get; set; }
        public DateTime? ComplainRecordDatetime { get; set; }
        public DateTime? LinkDownAt { get; set; }
        public DateTime? AccessPermissionDatetime { get; set; }
        public DateTime? ResulationTime { get; set; }
        public string? Comments { get; set; }
        public int? CouseID { get; set; }
        public string? SupportType { get; set; }

        [Key]
        public long? AutoID { get; set; }
    }
}
