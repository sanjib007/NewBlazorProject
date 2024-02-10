using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain
{
    public class HydraBalanceInfoModel
    {
        [Key]
        public string VC_CODE { get; set; }
        public string N_SUBJECT_ID { get; set; }
        public string CUSTOMER_STATUS { get; set; }
        public string BALANCE { get; set; }
    }
}
