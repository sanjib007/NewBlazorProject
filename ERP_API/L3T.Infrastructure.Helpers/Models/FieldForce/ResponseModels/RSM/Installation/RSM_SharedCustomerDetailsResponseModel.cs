using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    public class RSM_SharedCustomerDetailsResponseModel
    {
        [Key]
        public int ID { get; set; }
        public string CustomerID { get; set; }
        public string ONUMac { get; set; }
        public string ONUPort { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string Updateby { get; set; }
        public DateTime UpdateDate { get; set; }
        public string SharedCustomerID { get; set; }
        public string OnuOwnership { get; set; }
        public string SCRID { get; set; }
    }
}
