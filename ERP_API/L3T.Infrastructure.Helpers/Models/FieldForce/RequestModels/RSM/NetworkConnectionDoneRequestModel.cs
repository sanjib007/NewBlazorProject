using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels.RSM
{
    public class NetworkConnectionRequestModel
    {
        public string TicketNo { get; set; }
        public string UserId { get; set; }
        public string SubscriberCode { get; set; }
        public string SubscriberName { get; set; }
        public string Area { get; set; }
        public string BrCliCode { get; set; }
        public int BrSlNo { get; set; }
        public string CableNetworkText { get; set; }
        public int CableNetworkValue { get; set; }
        public string? CableNo { get; set; }
        public string? NttnNameText { get; set; }
        public int? NttnNameValue { get; set; }
        public string? TypeOfLinkText { get; set; }
        public int? TypeOfLinkValue { get; set; }
        public bool? CheckSharedLink { get; set; } = false;
        public bool? ScrIdP2MVisible { get; set; } = false;
        public string? ScrIdP2MText { get; set; }
        public string? OnuOwnerShip {  get; set; }
        public string? LinkIdP2MText { get; set; }
        public string? Remarks { get; set; }
        public string? BahonCoreId { get; set; }
        public string? Bahonlinkid { get; set; }
        public string? BtsNameText { get; set; }
        public int? BtsNameValue { get; set; }
        public string? SplitterName { get; set; }
        public string? FiberLaser { get; set; }
        public string? FiberPon { get; set;}
        public string? FiberPort { get; set; }
        public string? FiberOltBrand { get; set; }
        public string? FiberOltName { get; set; }
        public string? PortCapFB { get; set; }
        public string? LinkPath { get; set; }
        public string? shareOnuMac { get; set; }
        public string? ShareOnuPort { get; set; }
        public string? ShareOnuCustomerId { get; set; }
        public string? TeamName { get; set; }
        public string? SummitLinkId { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }





    }
}
