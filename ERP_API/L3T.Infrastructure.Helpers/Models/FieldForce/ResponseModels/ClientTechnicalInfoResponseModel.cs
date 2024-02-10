using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class ClientTechnicalInfoResponseModel
    {
        public string? TechInfoID { get; set; }
        public string? TrackingInfo { get; set; }
        public string? ClientCode { get; set; }
        public int? ClientSlNo { get; set; }
        public string? ClientAddrerssCode { get; set; }
        public string? real_ip { get; set; }
        public string? gate_way { get; set; }
        public string? subnet_musk { get; set; }
        public string? Pvlan { get; set; }
        public string? private_ip { get; set; }
        public string? private_gateway { get; set; }
        public string? private_musk { get; set; }
        public string? brDns1 { get; set; }
        public string? brDns2 { get; set; }
        public string? brsmtp { get; set; }
        public string? brpop3 { get; set; }
        public string? DeviceName { get; set; }
        public string? InternetBandwidthCIR { get; set; }
        public string? InternetBandwidthMIR { get; set; }
        public string? BandwidthType { get; set; }
        public string? VsatBandwidthDownCir { get; set; }
        public string? VsatBandwidthDownMir { get; set; }
        public string? VsatBandwidthUpCir { get; set; }
        public string? VsatBandwidthUpMir { get; set; }
        public string? LastmileBandwidth { get; set; }
        public string? IntercityBandwidth { get; set; }
        public string? NoteBandwith { get; set; }
        public string? bw_as_client { get; set; }
        public string? r_equip { get; set; }
        public string? r_rssi { get; set; }
        public string? r_snr { get; set; }
        public string? r_ThroughPutIntranetSingle { get; set; }
        public string? r_ThroughPutInternetSingle { get; set; }
        public string? r_ThroughPutIntranetMulti { get; set; }
        public string? r_ThroughPutInternetMulti { get; set; }
        public string? r_tower_hi { get; set; }
        public string? r_radio_ip { get; set; }
        public string? r_subnetmask { get; set; }
        public string? r_gateway { get; set; }
        public string? r_Bandwidth { get; set; }
        public string? r_vlan_id { get; set; }
        public string? r_air_distance { get; set; }
        public string? r_bts_sw_router { get; set; }
        public string? r_connected_to { get; set; }
        public string? r_note_for_radio { get; set; }
        public string? r_frequency { get; set; }
        public string? f_media_con { get; set; }
        public string? f_vlan_id { get; set; }
        public string? f_bts_sw_router { get; set; }
        public string? f_con_terminate_to { get; set; }
        public string? f_note_for_fiber { get; set; }
        public string? f_pon { get; set; }
        public string? f_port { get; set; }
        public string? f_splitter { get; set; }
        public string? f_mac { get; set; }
        public string? f_onu { get; set; }
        public string? f_oltchasisNo { get; set; }
        public string? f_onuNo { get; set; }
        public string? f_media_converter_switch_port { get; set; }
        public string? f_Laser { get; set; }
        public string? ClientIp { get; set; }
        public string? ClientGateway { get; set; }
        public string? ClientSubnetMusk { get; set; }
        [Key]
        public int sll { get; set; }
        public string? OldBWCIR { get; set; }
        public string? OLDBWMIR { get; set; }
        public int? ChangesStatus { get; set; }
        public string? LanPort { get; set; }
    }
}
