using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain
{
    public class RSM_SupportOfficeWiseIDModel
    {
        [Key]
        public int ID { get; set; }
        public string Team_ID { get; set; }
        public string Support_OfficeID { get; set; }
        public string UserID { get; set; }
    }
}
