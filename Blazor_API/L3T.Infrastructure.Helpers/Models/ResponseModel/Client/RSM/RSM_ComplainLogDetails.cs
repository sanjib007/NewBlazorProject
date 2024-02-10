using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel.Client.RSM
{
    
    public class RSM_ComplainLogDetails
    {
        [Key]
        public int ID { get; set; }
        public int? RefNo { get; set; }
        public string? UserID { get; set; }
        public string? Comments { get; set; }
        public DateTime? CommentsDate { get; set; }
        public string? ParseStatus { get; set; }
        public string? TokenID { get; set; }
    }
}
