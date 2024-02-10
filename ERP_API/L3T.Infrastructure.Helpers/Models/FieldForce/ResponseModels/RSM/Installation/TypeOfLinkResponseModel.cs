using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    public class TypeOfLinkResponseModel
    {
        [Key]
        public int Typeofp2mlinkID { get; set; }
        public string Typeofp2mlink { get; set; }
    }
}
