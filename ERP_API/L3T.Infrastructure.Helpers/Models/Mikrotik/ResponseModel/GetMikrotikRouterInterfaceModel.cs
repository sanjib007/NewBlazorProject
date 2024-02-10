using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.Mikrotik.ResponseModel
{
    public class GetMikrotikRouterInterfaceModel
    {
        public long Id { get; set; }
        public string ItemId { get; set; }
        public string? Name { get; set; }
        public string? DefaultName { get; set; }
        public string? Type { get; set; }
        public string? MTU { get; set; }
        public string? ActualMTU { get; set; }
        public string? L2MTU { get; set; }
        public string? MaxL2MTU { get; set; }
        public string? MacAddress { get; set; }
        public string? LastLinkDownTime { get; set; }
        public string? LastLinkUpTime { get; set; }
        public string? LinkDowns { get; set; }
        public string? RXByte { get; set; }
        public string? TXByte { get; set; }
        public string? RXPacket { get; set; }
        public string? TXPacket { get; set; }
        public string? RXDrop { get; set; }
        public string? TXDrop { get; set; }
        public string? TXQueueDrop { get; set; }
        public string? RXError { get; set; }
        public string? TXError { get; set; }
        public string? FpRxByte { get; set; }
        public string? FpTxByte { get; set; }
        public string? FpRxPacket { get; set; }
        public string? FpTxPacket { get; set; }
        public string? Running { get; set; }
        public string? Disabled { get; set; }
        public string? RouterIp { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
