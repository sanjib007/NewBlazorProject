using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    public class PendingInstallationInfoResponseModel
    {
        [Key]
        public string? RefNO { get; set; }
        public string? MqID { get; set; }
        public string? CustomerName { get; set; }
        public string? phone_no { get; set; }
        public DateTime? InitiateDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string? SalesPerson { get; set; }
        public string? Service { get; set; }
        public string? Area { get; set; }
        public string? brSupportOffice { get; set; }
        public string? Status { get; set; }
        public string? LastComments { get; set; }
        public DateTime? ScheduleDate { get; set; }
    }
}
