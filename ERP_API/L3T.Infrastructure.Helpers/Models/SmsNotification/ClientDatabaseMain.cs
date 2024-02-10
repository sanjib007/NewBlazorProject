using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.SmsNotification
{
    public class ClientDatabaseMain
    {
        [Key]
        public long num_row { get; set; }
        public string brCliCode { get; set; }
        public string phone_no { get; set; }
        public string MqID { get; set; }
    }
}
