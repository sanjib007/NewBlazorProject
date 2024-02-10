using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation

    {
    [Keyless]
    public class ContactDetailsResponseModel
    {
        public string? ContactName { get; set; }
        public string? HomePhone { get; set; }
        public string? WorkPhone { get; set; }
        public string? RegisteredMobile { get; set; }
        public string? RegisteredEmail { get; set; }
        public string? Fax { get; set; }
        public string? AltEmail { get; set; }
        public string? Designation { get; set; }
        public string? ComplementaryConnection	 { get; set; }
        public string? EmployeeID { get; set; }
        
    }
}
