using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cr.UI.Data.Permission
{
	public class SetPermissionForRoleOrUserRequestModel
	{
		public List<long> MenuId { get; set; }
		public string RoleName { get; set; }
		public string UserId { get; set; }
	}
}
