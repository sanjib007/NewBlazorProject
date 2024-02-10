using System.ComponentModel.DataAnnotations;

namespace IPV6ConfigSetup.DataAccess.Model.MISDBModel
{
    public class PackagePlanViewModel
    {
        [Key]
        public int PackageID { get; set; }
        public string PackageName { get; set; }
    }
}
