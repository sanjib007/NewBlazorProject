using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class tbl_ReasonForOutageResoponseModel
    {
        public long ID { get; set; }
        public string Reason { get; set; }
        public long ClosingNatureID { get; set; }        
        public DateTime InsertedDate { get; set; }
        public string InsertedBy { get; set; }
        public string? Common1 { get; set; }
        public string? Common2 { get; set; }
    }
}
