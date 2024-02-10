using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    [Keyless]
    public class SalesPersonInfoResponseModel
    {
        public string? SalesPerson { get; set; }
        public string? brCliCode { get; set; }
        public int? brSlNo { get; set; }
        public string? TKINo { get; set; }
        public string? SubscriberCode { get; set; }
        public string? SubscriberName	 { get; set; }
        public string? SupportOffice	 { get; set; }
        public string? BillDeliveryOption { get; set; }
        public string? SAFNumber { get; set; }
        public string? HydraID { get; set; }
        public string? Reference { get; set; }
    }
}
