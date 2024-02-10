using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel.Client
{
    [Keyless]
    public class RsmProfileResModel
    {
        public string? CustomerID { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? locaton { get; set; }
        public string? Area { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? OtherContact { get; set; }
        [NotMapped]
        public string? mac { get; set; }
        [NotMapped]
        public string? profileImage { get; set; }
        public string? Package { get; set; }
        public decimal? Balance { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal? BillingAmount { get; set; }
        public string? ServiceCode { get; set; }
        public string? MotherName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [NotMapped]
        public long? rewardPoint { get; set; }
       
        
    }
}
