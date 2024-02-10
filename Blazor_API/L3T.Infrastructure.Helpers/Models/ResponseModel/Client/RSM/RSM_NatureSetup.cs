using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel.Client.RSM
{
    [Keyless]
    public class RSM_NatureSetup
    {
        public int? NatureID { get; set; }
        public int? TaskTypeID { get; set; }
        public string? NatureName { get; set; }
        public string? Entryby { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? Team_ID { get; set; }
        public int? Status { get; set; }
        public int? MRC { get; set; }
    }
}
