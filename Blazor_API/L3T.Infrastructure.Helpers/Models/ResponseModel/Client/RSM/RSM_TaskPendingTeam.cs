using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel.Client.RSM
{
    

    public class RSM_TaskPendingTeam
    {
        [Key]
        public int ID { get; set; }
        public string? RefNo { get; set; }
        public string? PendingTeamID { get; set; }
        public int? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string? Completeby { get; set; }
    }
}
