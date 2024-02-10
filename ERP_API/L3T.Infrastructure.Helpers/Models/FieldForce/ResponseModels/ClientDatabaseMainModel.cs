using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class ClientDatabaseMainModel
    {
        [Key]
        public string brclicode {  get; set; }
        public string DistributorID { get; set; }
    }
}
