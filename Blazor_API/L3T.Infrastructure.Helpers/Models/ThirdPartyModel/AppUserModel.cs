﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ThirdPartyModel
{
	public class AppUserModel
	{
        public long Id { get; set; }
        public string fullName { get; set; }
        public string user_designation { get; set; }
        public string department { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string RoleName { get; set; }
    }
}
