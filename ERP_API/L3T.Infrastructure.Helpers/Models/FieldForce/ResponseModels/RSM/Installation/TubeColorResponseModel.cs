using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    public class TubeColorResponseModel
    {
        [Key]
        public int ColorID { get; set; }
        public string ColorName { get; set; }
    }
}
