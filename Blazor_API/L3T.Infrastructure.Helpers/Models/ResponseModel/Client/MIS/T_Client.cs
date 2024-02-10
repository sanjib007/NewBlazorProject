using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel.Client.MIS
{
    
    public class T_Client
    {
        [Key]
        public string client_ID { get; set; }
        public string? client_CompanyName { get; set; }
        public string? client_ContactPerson { get; set; }
        public DateTime? client_ContactDate { get; set; }
        public string? client_Location { get; set; }
        public string? client_Phone { get; set; }
        public string? client_Email { get; set; }
    }
}
