using L3T.Infrastructure.Helpers.Models.MISInstallation.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class InstallationTicketInformationViewModel
    {
        public SubscriptionInfoResponse SubscriptionInfo { get; set; }
        public List<PendingReasonResponseModel> PendingReasonResponse { get; set; }
        public int PendingReasonSelectedValue { get; set; }
        public List<Cli_PendingServiceNameModel> SendMailServiceLis { get; set; }
    }
}
