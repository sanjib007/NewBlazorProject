using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class ViewPendingServiceResponseModel
    {
        public string? Service { get; set; }
    }
}
