using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class RsmChecklistDetailsModel
    {

        [Key]
        public string? ClientID { get; set; }
        public int? brslno { get; set; }
        public int? OpticalLaser { get; set; }
        public int? ONUisConnectedwithCompatibleAdapter { get; set; }
        public int? ONUlabeledwithSubscriberID { get; set; }
        public int? Cat6cableused { get; set; }
        public int? ClientuseWiFiRouter { get; set; }
        public int? Deafultloginpasswordchanged { get; set; }
        public int? PasswrodisconfiguredasPrescribebyLink3 { get; set; }
        public int? RouterFirwareUptoDate { get; set; }
        public int? UpnpDiabled { get; set; }
        public int? WPSdisabled { get; set; }
        public int? WPA2securityEnabledtoAccessWiFi { get; set; }
        public int? RemoteManagementPortEnabled { get; set; }
        public int? RouterpositionedINproperplace { get; set; }
        public int? RouterisConnectedwithCompatibleAdapter { get; set; }
        public int? OnuportSpeedFE_GE { get; set; }
        public int? SpectrumAnalyzerusedRFchannelchecked { get; set; }
        public int? RouterType { get; set; }
        public int? ControllerOwner { get; set; }
        public int? GhzEnabled2_4 { get; set; }
        public int? SingleAP { get; set; }
        public int? MultipleAP { get; set; }
        public int? ChannelWidth { get; set; }
        public int? GhzEnabled5 { get; set; }
        public int? ChannelWidthAutoor40MHz { get; set; }
        public int? Channelbetween149_161AvilableAndSelected { get; set; }
        public int? Link3DNSusedinWANconfiguration { get; set; }
        public int? Link3DNSusedinDHCPconfiguration { get; set; }
        public int? RouterSupportIPv6 { get; set; }
        public int? RouterWanRecivedIPV6fromLink3 { get; set; }
        public int? LANdevicerecivingIPV6 { get; set; }
        public int? Canbrowseipv6 { get; set; }
        public int? NTPServer123 { get; set; }
        public int? RouterSupportScheduleReboot { get; set; }
        public int? ScheduleRebootConfigured { get; set; }
        public int? InternetSpeedTest { get; set; }
        public string? InternetSpeedUploadFile { get; set; }
        public int? BDIXSpeedTest { get; set; }
        public string? BDIXSpeedTestUploadFile { get; set; }
        public int? WifiAnalyzer2_4GHz { get; set; }
        public string? WifiAnalyzer2_4GHzUploadFile { get; set; }
        public int? WifiAnalyzer5GHz { get; set; }
        public string? WifiAnalyzer5GHzUploadFile { get; set; } 

    }
}
