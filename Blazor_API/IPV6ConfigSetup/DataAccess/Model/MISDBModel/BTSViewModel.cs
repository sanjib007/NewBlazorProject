using System.ComponentModel.DataAnnotations;

namespace IPV6ConfigSetup.DataAccess.Model.MISDBModel
{
    public class BTSViewModel
    {
        [Key]
        public int BtsSetupID { get; set; }
        public string BtsSetupName { get; set; }
    }
}
