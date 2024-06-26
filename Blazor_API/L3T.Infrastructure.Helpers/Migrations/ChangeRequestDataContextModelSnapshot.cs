﻿// <auto-generated />
using System;
using L3T.Infrastructure.Helpers.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace L3T.Infrastructure.Helpers.Migrations
{
    [DbContext(typeof(ChangeRequestDataContext))]
    partial class ChangeRequestDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities.AssignToEmployeeInfo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("CrId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("DeleteStatus")
                        .HasColumnType("bit");

                    b.Property<string>("EmpId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmpName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Task")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalDay")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AssignToEmployeeInfo");
                });

            modelBuilder.Entity("L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities.CRRequestResponseModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ErrorLog")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MethodName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Request")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestedIP")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Response")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CrRequestResponseModels");
                });

            modelBuilder.Entity("L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities.ChangeRequestedInfo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("AddReference")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AlternativeDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ApprovedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ApprovedUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AttachFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChangeFromExisting")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChangeImpactDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChangeRequestFor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChangeToAfter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CrDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("DeleteStatus")
                        .HasColumnType("bit");

                    b.Property<string>("DepartName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DevOpsTask")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployeeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ExpectedCompletedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FinalApprovedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FinalApprovedUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("InterdepartmentalApprovedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("InterdepartmentalApprovedUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Justification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LevelOfRisk")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LevelOfRiskDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestorDesignation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ChangeRequestedInfo");
                });

            modelBuilder.Entity("L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities.CrApprovalFlow", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("ApprovedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ApproverDepartment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApproverDesignation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApproverEmpId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApproverFlow")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApproverName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("CrId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Remark")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CrApprovalFlow");
                });

            modelBuilder.Entity("L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities.CrDefaultApprovalFlow", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("ApproverDepartment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApproverDesignation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApproverEmpId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApproverFlow")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApproverName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CrDefaultApprovalFlow");
                });

            modelBuilder.Entity("L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities.CrRemarkLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("CrId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("DeleteStatus")
                        .HasColumnType("bit");

                    b.Property<string>("EmpId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmpName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CrRemarkLogs");
                });

            modelBuilder.Entity("L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities.CrStatus", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("StatusImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CrStatus");
                });

            modelBuilder.Entity("L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities.TempChangeRequestedInfo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("AddReference")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AlternativeDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AttachFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChangeFromExisting")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChangeImpactDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChangeRequestFor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChangeToAfter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DepartName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Justification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LevelOfRisk")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LevelOfRiskDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestorDesignation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StepNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TempChangeRequestedInfo");
                });

            modelBuilder.Entity("L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel.SP_AssignEmployeeListResponse", b =>
                {
                    b.Property<int?>("CrId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("DeleteStatus")
                        .HasColumnType("bit");

                    b.Property<string>("EmpId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmpName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Task")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Total")
                        .HasColumnType("int");

                    b.Property<int?>("TotalDay")
                        .HasColumnType("int");

                    b.ToTable((string)null);

                    b.ToView("v_SP_AssignEmployeeListResponse", (string)null);
                });

            modelBuilder.Entity("L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel.SP_ChangeRequestListResponse", b =>
                {
                    b.Property<string>("AddReference")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AlternativeDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ApprovedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ApprovedUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AttachFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChangeFromExisting")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChangeImpactDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChangeRequestFor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChangeToAfter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CrDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("DeleteStatus")
                        .HasColumnType("bit");

                    b.Property<string>("DepartName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DevOpsTask")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ExpectedCompletedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FinalApprovedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FinalApprovedUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("InterdepartmentalApprovedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("InterdepartmentalApprovedUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Justification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LevelOfRisk")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LevelOfRiskDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestorDesignation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Total")
                        .HasColumnType("int");

                    b.ToTable((string)null);

                    b.ToView("v_SP_ChangeRequestListResponse", (string)null);
                });

            modelBuilder.Entity("L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel.StatusWiseTotalCrResponse", b =>
                {
                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Total")
                        .HasColumnType("int");

                    b.ToTable((string)null);

                    b.ToView("v_StatusWiseTotalCrResponse", (string)null);
                });

            modelBuilder.Entity("L3T.Infrastructure.Helpers.Models.SystemErrorLog.SystemErrorLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("ErrorDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ErrorMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("InsertedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MethodName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SystemErrorLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
