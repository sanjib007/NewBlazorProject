using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel;
using L3T.Infrastructure.Helpers.Models.ResponseModel;
using L3T.Infrastructure.Helpers.Models.SystemErrorLog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext
{
    public class ChangeRequestDataContext : DbContext
    {
        public ChangeRequestDataContext(DbContextOptions<ChangeRequestDataContext> options) : base(options)
        {
        }
        public DbSet<CRRequestResponseModel> CrRequestResponseModels { get; set; }
        public DbSet<TempChangeRequestedInfo> TempChangeRequestedInfo { get; set; }
        public DbSet<ChangeRequestedInfo> ChangeRequestedInfo { get; set; }
        public DbSet<AssignToEmployeeInfo> AssignToEmployeeInfo { get; set;}
        public DbSet<DeveloperInformation> DeveloperInformation { get; set; }
        public DbSet<CrRemarkLog> CrRemarkLogs { get; set; }
        public DbSet<SystemErrorLog> SystemErrorLogs { get; set; }
        public DbSet<CrApprovalFlow> CrApprovalFlow { get; set; }
        public DbSet<CrDefaultApprovalFlow> CrDefaultApprovalFlow { get; set; }
        public DbSet<CrStatus> CrStatus { get; set; }
        public DbSet<NotificationDetails> NotificationDetails { get; set; }
        public DbSet<CrAttatchedFile> CrAttatchedFiles { get; set; }
        public DbSet<ChangeRequestLog> ChangeRequestLogs { get; set; }



        // sp model
        public virtual DbSet<SP_ChangeRequestListResponse> ChangeRequestListResponse { get; set; }
        public virtual DbSet<StatusWiseTotalCrResponse> StatusWiseTotalCrResponse { get; set; }
        public virtual DbSet<SP_AssignEmployeeListResponse> AssignEmployeeListResponse { get; set; }
        public virtual DbSet<GetAllTotalCrByCatagoryWise> GetAllTotalCrByCatagoryWise { get; set; }
		public virtual DbSet<AutoUpdateCrApprovedToInProgressResponseModel> AutoUpdateCrApprovedToInProgress { get; set; }
		

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StatusWiseTotalCrResponse>().HasNoKey().ToView("v_StatusWiseTotalCrResponse");
            modelBuilder.Entity<SP_ChangeRequestListResponse>().HasNoKey().ToView("v_SP_ChangeRequestListResponse");
            modelBuilder.Entity<SP_AssignEmployeeListResponse>().HasNoKey().ToView("v_SP_AssignEmployeeListResponse");
            modelBuilder.Entity<GetAllTotalCrByCatagoryWise>().HasNoKey().ToView("v_GetAllTotalCrByCatagoryWise");
			modelBuilder.Entity<AutoUpdateCrApprovedToInProgressResponseModel>().HasNoKey().ToView("v_AutoUpdateCrApprovedToInProgress");


			base.OnModelCreating(modelBuilder);
        }


    }
}


// Add-Migration AssignToEmpUpdate -Context L3T.Infrastructure.Helpers.DataContext.ChangeRequestDataContext
// Update-Database -Context L3T.Infrastructure.Helpers.DataContext.ChangeRequestDataContext

//dotnet ef migrations add "HRISInit"  -p "L3T.Infrastructure.Helpers" -c  EmployeeDBContext -s "L3T.Employee.Service"
//dotnet ef database update  -p "L3T.Infrastructure.Helpers" -c  EmployeeDBContext -s "L3T.Employee.Service"
