using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class GetChangeEngListModel
    {
        [Key]
        public string Emp_id { get; set; }
        public string Employee_Name { get; set;}
    }
}
