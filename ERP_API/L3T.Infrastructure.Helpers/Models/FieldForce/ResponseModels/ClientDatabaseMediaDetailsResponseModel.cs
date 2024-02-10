using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class ClientDatabaseMediaDetailsResponseModel
    {
        public string brCliCode { get; set; }
        public Int32? brSlNo { get; set; }
        public string brCliAdrCode { get; set; }
        public Int32? WireID { get; set; }
        public Int32? TechID { get; set; }
        public Int32? MedID { get; set; }
        public string? Tempd { get; set; }
    }
}
