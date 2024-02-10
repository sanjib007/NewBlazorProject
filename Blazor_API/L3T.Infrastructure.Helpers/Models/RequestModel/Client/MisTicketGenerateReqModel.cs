using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.RequestModel.Client
{
    public class MisTicketGenerateReqModel
    {
        public string misSubscriberCode { get; set; }
        public int brslno { get; set; }
        public string misCategoryName { get; set; }
        public string complaincomments { get; set; }
        public string RelatedDepId { get; set; }
        public string BrAdrCode { get; set; }
        public string RelatedDeptName { get; set; }
        public string OfficeName { get; set; }
        public string Address { get; set; }
        public string str { get; set; }
        public string contact_det { get; set; }
        public string phone_no { get; set; }
        public string email_id { get; set; }
        public string brCategory { get; set; }
        public string brAreaGroup { get; set; }

    }
}
