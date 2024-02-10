using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ExtraModel
{
    public class MikrotikRouter
    {
        public string RouterIp { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
        public string CustomerIp { get; set; }
        public string? CallerId { get; set; }
    }
    public class Data
    {
        [JsonProperty(".id")]
        public string Id { get; set; }
        public string list { get; set; }
        public string address { get; set; }

        [JsonProperty("creation-time")]
        public string CreationTime { get; set; }
        public string dynamic { get; set; }
        public string disabled { get; set; }
        public string comment { get; set; }
    }

    public class GetUserInfo
    {
        public string status { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
        public List<Data> data { get; set; }
    }
}
