using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain
{
    public class tbl_SubSpliterEntryModel
    {
        [Key]
        public string EncloserNo { get; set; }
        public string? CableNo { get; set; }
    }
}
