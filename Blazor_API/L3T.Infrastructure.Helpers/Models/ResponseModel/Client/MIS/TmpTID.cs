using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel.Client.MIS
{
   
    public class TmpTID
    {
        [Key]
        public int? tid { get; set; }
        public string? dflg { get; set; }
        public string? pflg { get; set; }
       
    }
}
