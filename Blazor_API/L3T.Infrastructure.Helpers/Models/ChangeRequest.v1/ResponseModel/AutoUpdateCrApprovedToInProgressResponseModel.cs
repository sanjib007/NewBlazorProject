using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel
{
	
	public class AutoUpdateCrApprovedToInProgressResponseModel
	{
		public long Id { get; set; }
		public int CrId { get; set; }
		public string Subject { get; set; }
		public string Status { get; set; }
		public DateTime StartDate { get; set; } 
	}
}
