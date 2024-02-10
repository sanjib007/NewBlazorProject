using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class SupportOfficeModel
    {
        [Key]
        public int SupportOfficeID { get; set; }
        public string? SupportOfficeName { get; set; }
    }
}
