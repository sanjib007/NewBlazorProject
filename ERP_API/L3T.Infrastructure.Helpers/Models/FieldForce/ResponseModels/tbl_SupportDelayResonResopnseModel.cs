using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class tbl_SupportDelayResonResopnseModel
    {
        public long ID { get; set; }
        public string Reson { get; set; }
        public DateTime InsertedDate { get; set; }
        public string InsertedBy { get; set; }
        public string? Common1 { get; set; }
        public string? Common2 { get; set; }
    }
}
