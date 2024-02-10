using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain
{
    public class RSMTicketSelectedDataResponseModel
    {
        public List<string> CloseTicketSupportType {  get; set; }
        public List<string> ChangeSupportType { get; set; }
        public List<tbl_HelpDeskCategoryModel> FollowUpList { get; set; }
        public List<RSM_PendingTaskTypeModel> PendingReasonList { get; set; }
        public int PendingReasonSelectedId { get; set; }
        public bool CauseOfDelayVisible { get; set; } = false;
        public List<tbl_CasueOfDelayModel> PhoneSupportCasueOfDelayList { get;set; }
        public List<tbl_CasueOfDelayModel> PhysicalSupportCasueOfDelayList { get; set; }
        public string TicketTypeName { get; set; }
        public string NatureName { get; set; }
        public List<Rsm_tickettypeModel> TicketCategoriesList {  get; set; }
        public bool DevicecollectionstatusVisible { get; set; } = false;
        public List<string> DeviceCollection {  get; set; }
        public RSMSubcriberInformationResponseModel SubcriberInformation {  get; set; }
        public List<ComplainLogDetailsModel> TicketLog {  get; set; }
        public HydraPackageAndBalanceResponseModel HydraPackageAndBalanceInfo { get; set; }
        public ShowTechnicalInformationFromHydraModel TechinicalInformation {  get; set; }
    }
}
