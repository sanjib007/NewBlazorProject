using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.RequestModel.Client
{
    public class TicketCreateReqModel
    {
        public string subscriberCode { get; set; }
        public string natureId { get; set; }  // category Id
       // public string? dropcallId { get; set; }
        public string description { get; set; }
       // public string? priority { get; set; }

    }
}
