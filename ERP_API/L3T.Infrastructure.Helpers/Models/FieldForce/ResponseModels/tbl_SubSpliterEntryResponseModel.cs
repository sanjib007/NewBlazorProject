using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class tbl_SubSpliterEntryResponseModel
    {
        [Key]
        public int SpliterID { get; set; }
        public int BtsID { get; set; }
        public string OltName { get; set; }
        public int PON { get; set; }
        public int Port { get; set; }
        public int PortCapacity { get; set; }
        public int SpliterCapacity { get; set; }
        public string SpliterLocation { get; set; }
        public string CustomerName { get; set; }
        public string CableNo { get; set; }
        public string LinkPath { get; set; }
        public string Remarks { get; set; }
        public string EntryUserID { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateUserID { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CustomerCode { get; set; }
        public int CustomerSl { get; set; }
        public string Shifted { get; set; }
        public string EncloserNo { get; set; }
        public string OLTBrand { get; set; }
        public string UTPClient { get; set; }
    }
}
