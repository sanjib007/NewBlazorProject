using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class Cli_InstallationCompleteByTeamResponseModel
    {
        public int CompleteID { get; set; }
        public string TrackingInfo { get; set; }
        public string TeamName { get; set; }
        public DateTime CompleteDate { get; set; }
    }
}
