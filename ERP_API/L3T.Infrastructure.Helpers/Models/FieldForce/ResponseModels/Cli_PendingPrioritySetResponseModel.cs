using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class Cli_PendingPrioritySetResponseModel
    {
        public string PrioritySet { get; set; }
    }
}
