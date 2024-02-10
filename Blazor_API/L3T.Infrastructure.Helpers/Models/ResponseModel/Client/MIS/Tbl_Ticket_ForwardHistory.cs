using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel.Client.MIS
{
   
    public class Tbl_Ticket_ForwardHistory
    {

        public string? ticket_No { get; set; }
        public DateTime? ticket_ForwordTime { get; set; }
        public string? ticket_Forwarddatetime { get; set; }
        public DateTime? ticket_ForwardActualtime { get; set; }
        public string? ticket_ForwardBy { get; set; }
        public string? ticket_ForwardFromTeam { get; set; }
        public string? ticket_ForwardToTeam { get; set; }
        public string? ticket_ForwardToTeamName { get; set; }
        public string? ticket_ForwardInformingper { get; set; }
        public string? ticket_ForwardComments { get; set; }
        public DateTime? ticket_last_update { get; set; }
        public DateTime? ticket_postponed_time { get; set; }
        public int? ticket_postponed_hour { get; set; }
        public int? state { get; set; }

        [Key]
        public long ID { get; set; }
    }
}
