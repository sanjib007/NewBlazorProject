using System.ComponentModel.DataAnnotations;

namespace IPV6ConfigSetup.DataAccess.Model.MISDBModel
{
    public class DistributorViewModel
    {
        [Key]
        public int DistributorID { get; set; }
        public string DistributorName { get; set; }
    }
}
