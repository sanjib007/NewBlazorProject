using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.Mikrotik.ResponseModel
{
    public class MikrotikInterFaceDataModel
    {
        [JsonProperty(".id")]
        public string Id { get; set; }

        public string name { get; set; }

        [JsonProperty("default-name")]
        public string defaultname { get; set; }

        public string type { get; set; }

        public string mtu { get; set; }

        [JsonProperty("actual-mtu")]
        public string actualmtu { get; set; }

        public string l2mtu { get; set; }

        [JsonProperty("max-l2mtu")]
        public string maxl2mtu { get; set; }

        [JsonProperty("mac-address")]
        public string macaddress { get; set; }

        [JsonProperty("last-link-down-time")]
        public string lastlinkdowntime { get; set; }

        [JsonProperty("last-link-up-time")]
        public string lastlinkuptime { get; set; }

        [JsonProperty("rx-byte")]
        public string rxbyte { get; set; }

        [JsonProperty("tx-byte")]
        public string txbyte { get; set; }

        [JsonProperty("rx-packet")]
        public string rxpacket { get; set; }

        [JsonProperty("tx-packet")]
        public string txpacket { get; set; }

        [JsonProperty("rx-drop")]
        public string rxdrop { get; set; }

        [JsonProperty("tx-drop")]
        public string txdrop { get; set; }

        [JsonProperty("tx-queue-drop")]
        public string txqueuedrop { get; set; }

        [JsonProperty("rx-error")]
        public string rxerror { get; set; }

        [JsonProperty("tx-error")]
        public string txerror { get; set; }

        [JsonProperty("fp-rx-byte")]
        public string fprxbyte { get; set; }

        [JsonProperty("fp-tx-byte")]
        public string fptxbyte { get; set; }

        [JsonProperty("fp-rx-packet")]
        public string fprxpacket { get; set; }

        [JsonProperty("fp-tx-packet")]
        public string fptxpacket { get; set; }

        public string running { get; set; }
        
        public string disabled { get; set; }

        public string RouterIp { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
