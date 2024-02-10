using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain
{
    public class RSM_TaskAssignModel
    {
        [Key]
        public int ID { get; set; }
        public string? RefNo { get; set; }
        public string? UserID { get; set; }
        public DateTime? AssignDatetime { get; set; }
        public string? Assignby { get; set; }
        public string? TaskStatus { get; set; }
        public DateTime? TaskCompleteDate { get; set; }
        public string? TaskDetails { get; set; }

    }
}
