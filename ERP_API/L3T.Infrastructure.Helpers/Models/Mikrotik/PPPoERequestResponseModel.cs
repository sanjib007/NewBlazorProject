using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.Mikrotik
{
    public class PPPoERequestResponseModel
    {
        [Key]
        public long Id { get; set; }
        public string RouterIp { get; set; }
        public string CustomerIp { get; set; }
        public string MethordName { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string UserId { get; set; }
        public string SubId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
