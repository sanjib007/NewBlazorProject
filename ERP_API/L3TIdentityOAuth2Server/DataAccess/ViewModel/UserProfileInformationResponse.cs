using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace L3TIdentityOAuth2Server.DataAccess.ViewModel
{
    public class UserProfileInformation
    {
		[Key]
        public string? userid { get; set; }
		public string? user_name { get; set; }
		public string? user_designation { get; set; }
        public string? department { get; set; }
		public string? user_email { get; set; }
		public Int16? status { get; set; }
		public DateTime? resign_date { get; set; }
		public string? Emp_Mas_First_Name { get; set; }
		public string? Emp_Mas_Last_Name { get; set; }
		public string? Emp_Mas_Father_Name { get; set; }
		public string? Emp_Mas_Mother { get; set; }
		public string? Emp_Mas_Address { get; set; }
		public string? Emp_Mas_City { get; set; }
		public string? Emp_Mas_State { get; set; }
		public string? Emp_Mas_Post_Code { get; set; }
		public string? Emp_Mas_Country { get; set; }
		public string? emp_mas_Perm_Addr { get; set; }
		public string? emp_mas_Perm_City { get; set; }
		public string? emp_mas_Perm_state { get; set; }
		public string? emp_mas_Perm_Post { get; set; }
		public string? Emp_Mas_Perm_Country { get; set; }
		public string? Emp_Mas_Religion { get; set; }
		public DateTime? Emp_Mas_DOB { get; set; }
		public string? Emp_Mas_MaritalStatus { get; set; }
		public int? Emp_Mas_No_Children { get; set; }
		public string? Emp_Mas_Gender { get; set; }
		public string? Emp_Mas_Bloodgrp { get; set; }
		public string? Emp_Mas_Homephone { get; set; }
		public string? Emp_Mas_Workphone { get; set; }
		public string? Emp_Mas_HandSet { get; set; }
		public DateTime? Emp_Mas_Join_Date { get; set; }
		public DateTime? Emp_Mas_Confrim_Date { get; set; }
		public string? Emp_Mas_TIN { get; set; }
		public string? Sect { get; set; }
		public string? Office { get; set; }
        public string? WorkLocation { get; set; }
    }

    public class UserProfileInformationResponse
    {
        public string status { get; set; }
        public UserProfileInformation profile { get; set; }
    }
}
