using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class GeneralInfoDoneRequestModel
    {
        public string TicketId { get; set; }
        public string CliCode { get; set; }
        public string SlNo { get; set; }
        public string TeamName { get; set; }
        public DateTime CompleteDate { get; set; }
        public string brdns1 { get; set; }
        public string brdns2 { get; set; }
        public string brsmtp { get; set; }
        public string brpop3 { get; set; }
        public string SupportOfficeId { get; set; }
        public string SupportOfficeName { get; set; }
    }
}
