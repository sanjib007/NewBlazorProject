using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class MisCheckListRequestModel
    {
        public string? ClientID { get; set; }
        public string? brslno { get; set; }
        public string? OpticalLaser { get; set; }
        public string? ONUisConnectedwithCompatibleAdapter { get; set; }
        public string? ONUlabeledwithSubscriberID { get; set; }
        public string? Cat6cableused { get; set; }
        public string? ClientuseWiFiRouter { get; set; }
        public string? Deafultloginpasswordchanged { get; set; }
        public string? PasswrodisconfiguredasPrescribebyLink3 { get; set; }
        public string? RouterFirwareUptoDate { get; set; }
        public string? UpnpDiabled { get; set; }
        public string? WPSdisabled { get; set; }
        public string? WPA2securityEnabledtoAccessWiFi { get; set; }
        public string? RemoteManagementPortEnabled { get; set; }
        public string? RouterpositionedINproperplace { get; set; }
        public string? RouterisConnectedwithCompatibleAdapter { get; set; }
        public string? OnuportSpeedFE_GE { get; set; }
        public string? SpectrumAnalyzerusedRFchannelchecked { get; set; }
        public string? RouterType { get; set; }
        public string? ControllerOwner { get; set; }
        public string? GhzEnabled2_4 { get; set; }
        public string? SingleAP { get; set; }
        public string? MultipleAP { get; set; }
        public string? ChannelWidth { get; set; }
        public string? GhzEnabled5 { get; set; }
        public string? ChannelWidthAutoor40MHz { get; set; }
        public string? Channelbetween149_161AvilableAndSelected { get; set; }
        public string? Link3DNSusedinWANconfiguration { get; set; }
        public string? Link3DNSusedinDHCPconfiguration { get; set; }
        public string? RouterSupportIPv6 { get; set; }
        public string? RouterWanRecivedIPV6fromLink3 { get; set; }
        public string? LANdevicerecivingIPV6 { get; set; }
        public string? Canbrowseipv6 { get; set; }
        public string? NTPServer123 { get; set; }
        public string? RouterSupportScheduleReboot { get; set; }
        public string? ScheduleRebootConfigured { get; set; }
        public string? Reamarks { get; set; }
        public string? InternetSpeedTest { get; set; }
        public string? InternetSpeedRemarks { get; set; }
        public string? InternetSpeedFileName { get; set; }
        public string? InternetSpeedUploadFile { get; set; }
        public string? BDIXSpeedTest { get; set; }
        public string? BDIXSpeedTestRemarks { get; set; }
        public string? BDIXSpeedTestFileName { get; set; }
        public string? BDIXSpeedTestUploadFile { get; set; }
        public string? WifiAnalyzer2_4GHz { get; set; }
        public string? WifiAnalyzer2_4GHzRemarks { get; set; }
        public string? WifiAnalyzer2_4GHzFileName { get; set; }
        public string? WifiAnalyzer2_4GHzUploadFile { get; set; }
        public string? WifiAnalyzer5GHz { get; set; }
        public string? WifiAnalyzer5GHzRemarks { get; set; }
        public string? WifiAnalyzer5GHzFileName { get; set; }
        public string? WifiAnalyzer5GHzUploadFile { get; set; }
        public string? EntryBy { get; set; }
        public decimal? ActualRechargeAmount { get; set; }
        public string? Reason { get; set; }

        public string? TeamEqupmentStatus { get; set; }
        public string? ConnectedRouterFirewallSwitch { get; set; }
        public string? IpvlanbwStatus { get; set; }
        public string? ClientAccessSchedule { get; set; }
        public string? TowerStatusRadioLink { get; set; }
        public string? PhysicalMediaPrimaryLink { get; set; }
        public string? PatchcordCat6 { get; set; }
        public string? devicemmarkedprimarysecondary { get; set; }
        public string? upsbackup { get; set; }
        public string? PhysicalMediaSecondaryLink { get; set; }
        public string? RoutingStatus { get; set; }

        public string? GettingProperInterface { get; set; }
        public string? Testedallreachability { get; set; }

        public string? Testedallredundency { get; set; }

        public string? ConfigurationPartManaged { get; set; }
        public string? BwstatusFromMedia { get; set; }
        public string? ConfigureRouterProperly { get; set; }
        public string? WifiType { get; set; }
        public string? FrequencyBand { get; set; }
        public string? WiFiConfigurationManaged { get; set; }
        public string? RouterPositionokUserArea { get; set; }
        public string? LanCableNetworkExists { get; set; }
        public string? PacketLossGateway { get; set; }
        public string? bwstatuslan { get; set; }
        public string? LanProblem { get; set; }

        public string? RSSIinDbmExample10 { get; set; }
        public string? RSSIinDbmExample60 { get; set; }

        public string? ConnectedRouterFirewal { get; set; }
        public IFormFile InternetSpeedUploadFileDetails { get; set; }
        public IFormFile BDIXSpeedTestUploadFileDetails { get; set; }
        public IFormFile WifiAnalyzer2_4GHzUploadFileFileDetails { get; set; }
        public IFormFile WifiAnalyzer5GHzUploadFileDetails { get; set; }


    }
}
