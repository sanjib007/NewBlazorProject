using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cr.UI.Data.Permission
{
	public class GetAllMenuSetupAndPermissionRequestModel
	{
		public string UserId { get; set; }
		public string roleName { get; set; }
		public string? projectName { get; set; }

	}
}
