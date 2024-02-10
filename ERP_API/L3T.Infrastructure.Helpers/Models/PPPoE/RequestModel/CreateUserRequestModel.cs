using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.PPPoE.RequestModel
{
    public class CreateUserRequestModel
    {
        public string? username { get; set; }  // customer ID
        public string? package { get; set; }  // Starter, Simple 
        public string? password { get; set; }
        public string? ip { get; set; }
    }
}
