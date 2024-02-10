using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class t_EmployeeProjectModel
    {
        [Key]
        public int employeeProject_Id { get; set; }
        public string? employeeProject_Project_Id { get; set; }
        public string? employeeProject_Employee_Id { get; set; }
        public DateTime? employeeProject_AssigenDate { get; set; }
    }
}
