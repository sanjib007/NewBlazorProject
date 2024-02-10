using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel.Client.RSM
{
    [Keyless]
    public class tbl_user_info
    {
        public string? userid { get; set; }
        public string? user_name { get; set; }
        public string? user_designation { get; set; }
        public string? department_code { get; set; }
        public string? department { get; set; }
        public string? user_password { get; set; }
        public string? user_email { get; set; }
        public short? status { get; set; }
        public DateTime? last_update { get; set; }
        public DateTime? resign_date { get; set; }
        public int? managementid { get; set; }
        public int? Autosl { get; set; }
    }
}
