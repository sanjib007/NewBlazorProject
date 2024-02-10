using L3T.Infrastructure.Helpers.Models.SmsNotification.ReqponseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    [Keyless]
    public class PersonalDetailsResponseModel
    {
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MidleName { get; set; }
        public string? CompanyName { get; set; }
        public string? Nationality { get; set; }
        public string? IDProof	 { get; set; }
        public string? IDProofNo { get; set; }
        public string? Occupation { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? SpouseName { get; set; }
        public string? Gender { get; set; }
    }
}
