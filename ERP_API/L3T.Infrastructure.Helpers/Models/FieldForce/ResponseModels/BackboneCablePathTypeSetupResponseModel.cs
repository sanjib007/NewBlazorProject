using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class BackboneCablePathTypeSetupResponseModel
    {
        [Key]
        public Int32 CablePathID { get; set; }
        public string? CablePathName { get; set; }
    }
}
