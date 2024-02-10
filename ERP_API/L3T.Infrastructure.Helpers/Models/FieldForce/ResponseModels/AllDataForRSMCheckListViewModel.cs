using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class AllDataForRSMCheckListViewModel
    {
        public List<ChecklistResponseModel> ChecklistResponse { get; set; }
        public List<RouterTypeResponseModel> RouterTypeResponse { get; set; }
        public List<ControllerOwnerResponseModel> ControllerOwnerResponse { get; set; }
        public List<SingleApResponseModel> SingleApResponse { get; set; }
        public List<MultipleApResponseModel> MultipleApResponse { get; set; }
        public List<ChannelWidth20MHzResponseModel> ChannelWidth20MHzResponse { get; set; }
        public List<GhzEnabledResponseModel> GhzEnabledResponse { get; set; }
        public List<ChannelWidthAutoResponseModel> ChannelWidthAutoResponse { get; set; }
        public List<Channelbetween149_161ResponseModel> Channelbetween149_161Response { get; set; }
        public RsmChecklistDetailsModel RsmChecklistDetail { get; set; }
    }
}
