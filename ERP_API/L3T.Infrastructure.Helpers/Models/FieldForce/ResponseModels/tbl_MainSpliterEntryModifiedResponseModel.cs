using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class tbl_MainSpliterEntryModifiedResponseModel
    {
        public int? SplitterID { get; set; }
        public int? BtsID { get; set; }
        public string? OltName { get; set; }
        public int? PON { get; set; }
        public int? Port { get; set; }
        public int? PortCapacity { get; set; }
        public string? SpliterLeg { get; set; }
        public int? SplitterCapacity { get; set; }
        public string? SplitterLocation { get; set; }
        public string? EncloserNo { get; set; }
        public string? MoreSplitter { get; set; }
        public string? EntryUserID { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? OLTBrand { get; set; }
        public int? SynchStatus { get; set; }
        public string ViewText { get; set; }
    }
}
