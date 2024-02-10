using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class MisChecklistDetailsB2BResponseModel
    {
        [Key]
        public string? ClientID { get; set; }
        public int? brslno { get; set; }
        public int? TeamEqupmentStatus { get; set; }
        public int? IpvlanbwStatus { get; set; }
        public int? ClientAccessSchedule { get; set; }
        public int? TowerStatusRadioLink { get; set; }
        public int? PhysicalMediaPrimaryLink { get; set; }
        public int? PhysicalMediaSecondaryLink { get; set; }
        public int? PatchcordCat6 { get; set; }
        public string? ConnectedRouterFirewal { get; set; }
        public int? Devicemmarkedprimarysecondary { get; set; }
        public int? Upsbackup { get; set; }
        public int? RoutingStatus { get; set; }
        public int? ConfigurationPartManaged { get; set; }
        public int? GettingProperInterface { get; set; }
        public int? ConfigureRouterProperly { get; set; }
        public int? Testedallreachability { get; set; }
        public int? Testedallredundency { get; set; }
        public int? BwstatusFromMedia { get; set; }
        public int? LanCableNetworkExists { get; set; }
        public int? PacketLossGateway { get; set; }
        public int? bwstatuslan { get; set; }
        public int? LanProblem { get; set; }
        public int? WifiType { get; set; }
        public int? WiFiConfigurationManaged { get; set; }
        public int? FrequencyBand { get; set; }
        public int? RouterPositionokUserArea { get; set; }
        public int? GhzEnabled2_4 { get; set; }
        public int? SingleAP { get; set; }
        public int? MultipleAP { get; set; }
        public int? ChannelWidth { get; set; }
        public int? GhzEnabled5 { get; set; }
        public int? ChannelWidthAutoor40MHz { get; set; }
        public int? Channelbetween149_161AvilableAndSelected { get; set; }
        public int? RouterFirwareUptoDate { get; set; }
        public int? RemoteManagementPortEnabled { get; set; }
        public int? InternetSpeedTest { get; set; }
        public string? InternetSpeedUploadFile { get; set; }
        public int? BDIXSpeedTest { get; set; }
        public string? BDIXSpeedTestUploadFile { get; set; }
        public int? WifiAnalyzer2_4GHz { get; set; }
        public string? Reamarks { get; set; }
        public int? WifiAnalyzer5GHz { get; set; }
        public string? RSSIinDbmExample10 { get; set; } 
        public string? RSSIinDbmExample60 { get; set; }
    }
}
