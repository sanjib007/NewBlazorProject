using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.Mikrotik.ResponseModel
{
    public class GetMikrotikRouterUserInfoResponseModel
    {
        [Key]
        public long MikrotikRouterUserInfoId { get; set; }
        [JsonProperty(".id")]
        public string Id { get; set; }
        public string list { get; set; }
        public string address { get; set; }

        [JsonProperty("creation-time")]
        public string CreationTime { get; set; }
        public string dynamic { get; set; }
        public string disabled { get; set; }
        public string comment { get; set; }
        public string RouterIp { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserId { get; set; }
        public string? UniqueId { get; set; }
    }
}
