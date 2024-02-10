using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class SP_ForwardTicketApiModel
    {
        [Key]
        public string TicketId { get; set; }
        public string UserId { get; set; }
        public string SuccessOrErrorMessage { get; set; }
    }
}
