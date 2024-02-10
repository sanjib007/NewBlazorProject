using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class TTaskModel
    {
        [Key]
        public string task_Id {  get; set; }
        public string? task_type { get; set; }
        public string task_Name { get; set; }
        public string? task_Description { get; set; }
        public string task_Project_Title { get; set; }
        public DateTime task_StartDate { get; set; }
        public DateTime? task_EstimateTime { get; set; }
        public string task_Employee_Name { get; set; }
        public string task_Project_Id { get; set; }
        public string task_Employee_Id { get; set; }
        public string? task_Status { get; set; }
    }
}
