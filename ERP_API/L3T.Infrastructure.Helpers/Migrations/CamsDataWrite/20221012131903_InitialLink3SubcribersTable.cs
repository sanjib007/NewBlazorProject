using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L3T.Infrastructure.Helpers.Migrations.CamsDataWrite
{
    public partial class InitialLink3SubcribersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Link3Subscribers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriberID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sub_password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriberStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Start_date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Start_date_new = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expire_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PackagePlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IP_Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link3_Entity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Up_date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Up_date_new = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ONU_Serial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerStatus = table.Column<int>(type: "int", nullable: false),
                    Upd_stat = table.Column<int>(type: "int", nullable: false),
                    TerminateStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Onu_Recevied = table.Column<int>(type: "int", nullable: false),
                    Onu_Lost = table.Column<int>(type: "int", nullable: false),
                    Update_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Jcon_status = table.Column<int>(type: "int", nullable: false),
                    MIS_ActiveDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActiveDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangeTypeID = table.Column<int>(type: "int", nullable: false),
                    TransactionID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobAction = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Link3Subscribers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Link3Subscribers");
        }
    }
}
