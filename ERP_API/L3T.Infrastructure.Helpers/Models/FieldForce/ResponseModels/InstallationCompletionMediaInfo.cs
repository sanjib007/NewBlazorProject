using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class InstallationCompletionMediaInfo
    {
        public string MediaName { get; set; }
        public string brCliCode { get; set; }
        public int brSlNo { get; set; }
        public string brCliAdrCode { get; set; }

    }
}
