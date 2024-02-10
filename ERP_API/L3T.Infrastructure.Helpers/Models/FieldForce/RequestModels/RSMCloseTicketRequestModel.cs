using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class RSMCloseTicketRequestModel
    {
        public string TicketId { get; set; }
        public string CustomerID { get; set; }
        public string SupportType { get; set; }
        public int TicketCategory { get; set; }
        public int ChangeNature { get; set; }        
        public string? Comment { get; set;}
        public DateTime? RestoredDateTime { get; set;}
        public bool? CauseofdelayVisible { get; set;}
        public int? CauseofdelayValue {  get; set;}
        public bool? DevicecollectionstatusVisible { get; set;}
        public string? DeviceCollectionValue { get; set;}
        public string? RestoreMobileNo { get; set;}
        public string? Color { get; set;}
        public string? TokenID { get; set;}
        public decimal? Balance { get; set;}
    }
}
