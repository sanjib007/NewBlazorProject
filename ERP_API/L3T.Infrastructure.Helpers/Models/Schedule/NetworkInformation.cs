using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.Schedule
{
	public class NetworkInformation
	{
		[Key]
		public long ID {  get; set; }
		public string CustomerID { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string Area { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Packages { get; set; }
		public string AssignedPackage { get; set; }
		public string Package { get; set; }
		public DateTime ConnectionStartDate { get; set; }
		public string BrasBts { get; set; }
		public string BrasIP { get; set; }
		public string IPv4 { get; set; }
		public string Subnet { get; set; }
		public string IPv6 { get; set; }
		public string VLAN { get; set; }
		public string OLTPON { get; set; }
		public string OLT { get; set; }
		public string BTS { get; set; }
		public string MACAddress { get; set; }
		public string CustomerStatus { get; set; }
		public string ServiceStatus { get; set; }
		public DateTime ServiceExpireDate { get; set; }
		public string SupportOffice { get; set; }
		public string CustomerCategory { get; set; }
		public string PoliceStation { get; set; }
		public DateTime DOB { get; set; }
		public int Status { get; set; }
	}
}
