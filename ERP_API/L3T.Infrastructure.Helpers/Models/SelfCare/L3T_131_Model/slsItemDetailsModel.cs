using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.SelfCare.L3T_131_Model
{
    public class slsItemDetailsModel
    {
        [Key]
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public decimal ItemSalesPrice { get; set; }
        public char? IsActive { get; set; }
    }
}
