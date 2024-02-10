using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel.Client.MIS
{
    
    public class Tbl_client_com_det
    {
        public string? com_ref_no { get; set; }
        public string? client_id { get; set; }

        [Key]
        public int sl_no { get; set; }
        public string? Area { get; set; }
        public string? Medea { get; set; }
        public string? Bts { get; set; }
        public string? cat { get; set; }
    }
}
