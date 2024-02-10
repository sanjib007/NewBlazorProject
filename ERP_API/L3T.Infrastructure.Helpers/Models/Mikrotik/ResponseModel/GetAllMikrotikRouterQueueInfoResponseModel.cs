using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.Mikrotik.ResponseModel
{
    public class GetAllMikrotikRouterQueueInfoResponseModel
    {
        [JsonProperty(".id")]
        public string id { get; set; }
        public string name { get; set; }
        public string target { get; set; }
        public string parent { get; set; }
        public string priority { get; set; }
        public string queue { get; set; }

        [JsonProperty("limit-at")]
        public string limitat { get; set; }

        [JsonProperty("max-limit")]
        public string maxlimit { get; set; }

        [JsonProperty("burst-limit")]
        public string burstlimit { get; set; }

        [JsonProperty("burst-threshold")]
        public string burstthreshold { get; set; }

        [JsonProperty("burst-time")]
        public string bursttime { get; set; }

        [JsonProperty("bucket-size")]
        public string bucketsize { get; set; }
        public string bytes { get; set; }

        [JsonProperty("total-bytes")]
        public string totalbytes { get; set; }
        public string packets { get; set; }

        [JsonProperty("total-packets")]
        public string totalpackets { get; set; }
        public string dropped { get; set; }

        [JsonProperty("total-dropped")]
        public string totaldropped { get; set; }
        public string rate { get; set; }

        [JsonProperty("total-rate")]
        public string totalrate { get; set; }

        [JsonProperty("packet-rate")]
        public string packetrate { get; set; }

        [JsonProperty("total-packet-rate")]
        public string totalpacketrate { get; set; }

        [JsonProperty("queued-packets")]
        public string queuedpackets { get; set; }

        [JsonProperty("total-queued-packets")]
        public string totalqueuedpackets { get; set; }

        [JsonProperty("queued-bytes")]
        public string queuedbytes { get; set; }

        [JsonProperty("total-queued-bytes")]
        public string totalqueuedbytes { get; set; }
        public string invalid { get; set; }
        public string dynamic { get; set; }
        public string disabled { get; set; }
        public string RouterIp { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
