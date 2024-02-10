using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    [Keyless]
    public class SupportOfficeInfoRequestModel
    {
        public Int32 brSupportOfficeId { get; set; }
        public string brSupportOffice { get; set; }
        public string brCliCode { get; set; }
        public Int32 brSlNo { get; set; }
        public string TrackingInfo { get; set; }

    }
}
