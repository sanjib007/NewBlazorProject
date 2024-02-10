using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    public class tbl_BackboneFiber_HomeLinkResponseModel
    {
        [Key]
        public int Autoid { get; set; }
        public string? SCRID { get; set; }
        public int? LinkTypeId { get; set; }
        public int? ConnectionTypeMC { get; set; }
        public int? BTSID { get; set; }
        public string? ConnectionType { get; set; }
        public string? FromLocation { get; set; }
        public int? FromLDPId { get; set; }
        public string? ToLocation { get; set; }
        public int? ToLDPId { get; set; }
        public int? NoOfCore { get; set; }
        public decimal? Capacity { get; set; }
        public int? UsingStatus { get; set; }
        public DateTime? DateofInstallation { get; set; }
        public string? BillingDistancetype { get; set; }
        public string? BillingType { get; set; }
        public int? LinkDistancepercore { get; set; }
        public string? Lastmilemc { get; set; }
        public decimal? OTC { get; set; }
        public decimal? Amountper { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? Status { get; set; }
        public string? Entryby { get; set; }
        public DateTime? Entrydate { get; set; }
        public string? Remarks { get; set; }
        public DateTime? TerminateDate { get; set; }
    }
}
