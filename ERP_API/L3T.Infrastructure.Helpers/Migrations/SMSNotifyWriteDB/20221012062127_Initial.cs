using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L3T.Infrastructure.Helpers.Migrations.SMSNotifyWriteDB
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmsNotification",
                columns: table => new
                {
                    SmsNotificationID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    Sender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reciver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rcv_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SMS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cust_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TKT_CR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsNotification", x => x.SmsNotificationID);
                });

            migrationBuilder.CreateTable(
                name: "SmsNotifyRequestResponse",
                columns: table => new
                {
                    SmsNotifyRequestResponseId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Request = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsNotifyRequestResponse", x => x.SmsNotifyRequestResponseId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmsNotification");

            migrationBuilder.DropTable(
                name: "SmsNotifyRequestResponse");
        }
    }
}
