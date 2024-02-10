using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class CustomerInfoResponseModel
    {
        [Key]
        public string HeadOfficeName { get; set; }
        public string brName { get; set; }
        public string brCliCode { get; set; }
        public int brSlNo { get; set; }
        [NotMapped]
        public string? customer { get; set; }
    }
}
