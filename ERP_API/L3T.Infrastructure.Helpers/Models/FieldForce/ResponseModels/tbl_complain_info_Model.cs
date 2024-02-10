using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class tbl_complain_info_Model
    {
        [Key]
        public string comp_info_ref_no { get; set; }
        public string? comp_info_con_email { get; set; }
        public string? comp_info_com_name { get; set; }
    }
}
