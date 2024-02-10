using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain
{
    public class tbl_team_mem_permissionModel
    {
        [Key]
        public int AutoSL { get; set; }
        public string Team_id { get; set; }
        public string Team_Name { get; set; }
        public string Emp_id { get; set; }
        public string Emp_name { get; set; }
        public string Assign_emp { get; set; }
        public string Post_reply { get; set; }
        public string Final_post { get; set; }
        public string Close_task { get; set; }
        public string Database_Update { get; set; }
        public string Ticket_open { get; set; }
        public string Ticket_forward { get; set; }
        public string Ticket_close { get; set; }
        public string Ticket_insSolve { get; set; }
        public string Ticket_viewAll { get; set; }
        public string LockUnlock_Prm { get; set; }
        
    }
}
