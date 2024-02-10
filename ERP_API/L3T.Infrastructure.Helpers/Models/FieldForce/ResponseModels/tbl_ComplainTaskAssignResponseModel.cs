using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class tbl_ComplainTaskAssignResponseModel
    {
        [Key]
        public int ID { get; set; }
        public string? Refno { get; set; }
        public string? UserID { get; set; }
        public DateTime AssignDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public int Status { get; set; }
    }
}
