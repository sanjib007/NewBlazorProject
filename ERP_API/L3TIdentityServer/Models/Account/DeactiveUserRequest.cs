using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace L3TIdentityServer.Models.Account
{
    public class DeactiveUserRequest
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }
    }
}
