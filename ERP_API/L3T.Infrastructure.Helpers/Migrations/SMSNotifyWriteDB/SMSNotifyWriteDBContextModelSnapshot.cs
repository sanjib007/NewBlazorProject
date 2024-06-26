﻿// <auto-generated />
using System;
using L3T.Infrastructure.Helpers.DataContext.SMSNotifyDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace L3T.Infrastructure.Helpers.Migrations.SMSNotifyWriteDB
{
    [DbContext(typeof(SMSNotifyWriteDBContext))]
    partial class SMSNotifyWriteDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("L3T.Infrastructure.Helpers.Models.SMSNotify.SmsNotification", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"), 1L, 1);

                    b.Property<string>("AddBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Cust_ID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("MessageId")
                        .HasColumnType("bigint");

                    b.Property<string>("NewComment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("New_Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RSM_STAT")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rcv_Date_Bl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reciver")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SMS")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TKT_CR")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TtyCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("VIP")
                        .HasColumnType("int");

                    b.Property<int?>("acknowledge")
                        .HasColumnType("int");

                    b.Property<string>("address_by")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("call")
                        .HasColumnType("int");

                    b.Property<int?>("call_bk")
                        .HasColumnType("int");

                    b.Property<string>("called")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("date_convert")
                        .HasColumnType("datetime2");

                    b.Property<string>("frm")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("link_stat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("msg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("my_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("rcv_date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("rcv_time")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("replied_msg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("sms_replied")
                        .HasColumnType("int");

                    b.Property<int?>("snd_sms")
                        .HasColumnType("int");

                    b.Property<int?>("stat")
                        .HasColumnType("int");

                    b.Property<string>("sys_time")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ticket_cr")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("SmsNotification");
                });

            modelBuilder.Entity("L3T.Infrastructure.Helpers.Models.SMSNotify.SmsNotifyRequestResponse", b =>
                {
                    b.Property<long>("SmsNotifyRequestResponseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("SmsNotifyRequestResponseId"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("MethordName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Request")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Response")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SmsNotifyRequestResponseId");

                    b.ToTable("SmsNotifyRequestResponse");
                });
#pragma warning restore 612, 618
        }
    }
}
