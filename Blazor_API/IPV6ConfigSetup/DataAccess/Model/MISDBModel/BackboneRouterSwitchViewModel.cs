using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IPV6ConfigSetup.DataAccess.Model.MISDBModel
{
    public class BackboneRouterSwitchViewModel
    {
        [Key]
        public int RouterSwitchID { get; set; }
        public string RouterSwitchBrand { get; set;}
    }

    [Keyless]
    public class BackboneRouterSwitchInformationViewModel
    {
        public string RouterSwitchModel { get; set; }
        public string HostName { get; set; }
        public string RouterSwitchIP { get; set; }
        public string Noofport { get; set; }
    }
}
