using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.SmsNotification
{
    public class raw_data
    {
        [Key]
        public int Id { get; set; }
        public string? frm { get; set; }
        public string? rcv_date { get; set; }
        public string? rcv_time { get; set; }
        public string? msg { get; set; }
        public int? stat { get; set; }
        public int? call { get; set; }
        public int? snd_sms { get; set; }
        public int? call_bk { get; set; }
        public string? cust_id { get; set; }
        public string? sys_time { get; set; }
        public string? called { get; set; }
        public string? link_stat { get; set; }
        public string? address_by { get; set; }
        public string? ticket_cr { get; set; }
        public byte[]? comment { get; set; }
        public DateTime? date_convert { get; set; }

        public int? acknowledge { get; set; }
        public int? sms_replied { get; set; }
        public int? VIP { get; set; }
        public string? replied_msg { get; set; }
        public DateTime? my_date { get; set; }
        public string? RSM_STAT { get; set; }
        public string? New_Comment { get; set; }            
    }
}
