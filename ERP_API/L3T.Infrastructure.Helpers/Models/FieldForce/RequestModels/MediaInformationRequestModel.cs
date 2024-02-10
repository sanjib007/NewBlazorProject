using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    [Keyless]
    public class MediaInformationRequestModel
    {
        public string brCliCode { get; set; }
        public string brSlNo { get; set; }
        public string brAdrNewCode { get; set; }
        public string internetTechnologyWireId { get; set; }
        public string internetMediaTechId { get; set; }
        public string internetMediaId { get; set; }
        public string? TrackingInfo { get; set; }
        public string? internetMediaTypeName { get; set; }
    }
}
