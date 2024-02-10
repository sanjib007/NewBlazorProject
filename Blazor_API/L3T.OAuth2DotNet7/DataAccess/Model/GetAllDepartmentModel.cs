using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.OAuth2DotNet7.DataAccess.Model
{
    public class GetAllDepartmentModel
    {
        [Key]
        public string Department { get; set; }
    }
}
