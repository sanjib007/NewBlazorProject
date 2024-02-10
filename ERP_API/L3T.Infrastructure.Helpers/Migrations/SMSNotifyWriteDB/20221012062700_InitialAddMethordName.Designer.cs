﻿// <auto-generated />
using System;
using L3T.Infrastructure.Helpers.DataContext.SMSNotifyDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace L3T.Infrastructure.Helpers.Migrations.SMSNotifyWriteDB
{
    [DbContext(typeof(SMSNotifyWriteDBContext))]
    [Migration("20221012062700_InitialAddMethordName")]
    partial class InitialAddMethordName
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("L3T.Infrastructure.Helpers.Models.SMSNotify.SmsNotification", b =>
                {
                    b.Property<long>("SmsNotificationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("SmsNotificationID"), 1L, 1);

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

                    b.Property<DateTime?>("Rcv_Date")
                        .HasColumnType("datetime2");

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

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("SmsNotificationID");

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