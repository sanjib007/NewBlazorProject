using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel.Client.MIS
{
    [Keyless]
    public class Tbl_Com_Category
    {
        public decimal? C_id { get; set; }
        public string? Com_Category { get; set; }
        public string? SelfCategory { get; set; }
        public string? RelatedDepID { get; set; }
        public int? IsNetwork { get; set; }
        public string? RelatedDeptName { get; set; }
    }
}
