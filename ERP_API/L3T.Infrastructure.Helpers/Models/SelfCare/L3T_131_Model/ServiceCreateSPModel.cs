using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.SelfCare.L3T_131_Model
{
    public class ServiceCreateSPModel
    {
        [Key]
        public string? SalesId { get; set; }
        public int Code { get; set; }
    }
}
