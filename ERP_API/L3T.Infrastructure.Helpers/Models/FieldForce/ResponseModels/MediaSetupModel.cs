using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class MediaSetupModel
    {
        [Key]
        public int MedID { get; set; }
        public string? MediaName { get; set; }
    }
}
