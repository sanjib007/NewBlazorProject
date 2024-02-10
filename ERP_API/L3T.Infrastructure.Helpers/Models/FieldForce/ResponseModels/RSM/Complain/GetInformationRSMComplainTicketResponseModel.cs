using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain
{
    public class GetInformationRSMComplainTicketResponseModel
    {
        [Key]
        public string Subscriber_Code { get; set; }
        public string? IP { get; set; }
        public string? Subscriber_Name { get; set; }
        public string? Gateway { get; set; }
        public string? Address { get; set; }
        public string? Subnet_Mask { get; set; }
        public string? Area { get; set; }
        public string? VLAN { get; set; }
        public string? Contact_Detail { get; set; }
        public string? Contact_Number { get; set; }
        public string? OLT { get; set; }
        public string? OLT_IP { get; set; }
        public string? Email { get; set; }
        public string? PON { get; set; }
        public string? Support_Office { get; set; }
        public string? PORT { get; set; }
        public string? Splitter { get; set; }
        public string? Customer_MAC { get; set; }
        public string? ONU_MAC { get; set; }
        public string? ONU_PORT { get; set; }
        public string? ONU_ID { get; set; }
        public string? District { get; set; }
        public string? UpazilaThana { get; set; }
        public string? Post_Code { get; set; }
        public string? AreaGroup { get; set; }
        public string? House_Name { get; set; }
        public string? House_No { get; set; }
        public string? FloorFlat_No { get; set; }
        public string? Road_Name { get; set; }
        public string? Road_No { get; set; }
        public string? Block_No { get; set; }
        public string? Section { get; set; }
        public string? Sector { get; set; }
        public string? Land_Mark { get; set; }
    }
}
