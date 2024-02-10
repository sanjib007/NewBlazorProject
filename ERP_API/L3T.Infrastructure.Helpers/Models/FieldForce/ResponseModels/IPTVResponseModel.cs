using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class IPTVResponseModel
    {
        [Key]
        public int AutoID { get; set; }
        public string? MISID { get; set; }
        public string? JaconCustomerID { get; set; }
        public string? PackageID { get; set; }
        public string? SmartCardID { get; set; }
        public string? SettopBoxID { get; set; }
        public string? MacAddress { get; set; }
        public string? Vlan { get; set; }
        public string? OnuPort { get; set; }
        public string? OnuModel { get; set; }
        public string? IP { get; set; }
        public string? Gateway { get; set; }
        public string? Subnetmask { get; set; }
        public string? Remarks { get; set; }
        public int? Status { get; set; }
        public string? device_paired_conax_id { get; set; }
        public string? device_tivo_id { get; set; }
        public string? MQ_ID_MIS_ID { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? Entrydate { get; set; }
        public string? EmporSubscriber { get; set; }
        public int? PairConaxID { get; set; }
        public int? TivoID { get; set; }
        public string? Pair_Conax { get; set; }
        public string? TivoName { get; set; }
        public int? ChannelQty { get; set; }
        public decimal? MRC { get; set; }
        public decimal? OTC { get; set; }
        public string? Commets { get; set; }
    }
}
