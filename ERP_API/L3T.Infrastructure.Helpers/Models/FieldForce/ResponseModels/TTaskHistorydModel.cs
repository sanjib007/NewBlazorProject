using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class TTaskHistoryModel
    {
        [Key]
        public string taskHistory_Task_Id { get; set; }
        public string taskHistory_Employee_Id_AssignedTo { get; set; }
        public string taskHistory_Project_Id { get; set; }
        public DateTime taskHistory_TaskAsignDate { get; set; }
        public string taskHistory_Employee_Id_AssignedBy { get; set; }
    }
}
