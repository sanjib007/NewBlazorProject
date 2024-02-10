using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class ClientStatusSlaResponseModel
    {
        public int ClientStatusSlaID { get; set; }
        public string? StatusName { get; set; }
    }
}
