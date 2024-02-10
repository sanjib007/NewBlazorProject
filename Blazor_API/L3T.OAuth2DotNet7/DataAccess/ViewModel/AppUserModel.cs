using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.OAuth2DotNet7.DataAccess.ViewModel
{
	public class AppUserModel
	{
		[Key]
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
