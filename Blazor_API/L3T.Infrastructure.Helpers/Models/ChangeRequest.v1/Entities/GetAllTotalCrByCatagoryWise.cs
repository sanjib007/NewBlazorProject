using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities
{
    public class GetAllTotalCrByCatagoryWise
    {
        [Key]
        public string ChangeRequestFor { get; set; }
        public int TotalItem { get; set; }
    }
}
