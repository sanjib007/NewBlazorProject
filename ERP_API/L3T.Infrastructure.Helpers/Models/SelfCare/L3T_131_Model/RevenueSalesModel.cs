using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.SelfCare.L3T_131_Model
{
    public class RevenueSalesModel
    {
        [Key]
        public string SalesID { get; set; }
        public string ClientID { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceNarration { get; set; }
        public decimal BillingServiceRate { get; set; }
        public DateTime NextBillingMonth { get; set; }
        public DateTime UsagesStartDate { get; set; }
        public DateTime UsagesEndDate { get; set; }
    }
}
