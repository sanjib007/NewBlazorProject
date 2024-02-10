using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.SelfCare.Wfa2_133_Model
{
    public class VasProductsModel
    {
        [Key]
        public long ID { get; set; }
        public decimal Price { get; set; }
        public int StatusID { get; set; }
        public string? ProductName { get; set; }
        public string? ServiceCode { get; set; }
    }
}
