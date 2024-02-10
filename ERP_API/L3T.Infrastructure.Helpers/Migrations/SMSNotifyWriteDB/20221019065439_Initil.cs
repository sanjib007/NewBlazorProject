using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L3T.Infrastructure.Helpers.Migrations.SMSNotifyWriteDB
{
    public partial class Initil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmsNotification",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    Sender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reciver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rcv_Date_Bl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SMS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cust_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TKT_CR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TtyCount = table.Column<int>(type: "int", nullable: false),
                    frm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rcv_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rcv_time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    msg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stat = table.Column<int>(type: "int", nullable: true),
                    call = table.Column<int>(type: "int", nullable: true),
                    snd_sms = table.Column<int>(type: "int", nullable: true),
                    call_bk = table.Column<int>(type: "int", nullable: true),
                    sys_time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    called = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    link_stat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ticket_cr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date_convert = table.Column<DateTime>(type: "datetime2", nullable: true),
                    acknowledge = table.Column<int>(type: "int", nullable: true),
                    sms_replied = table.Column<int>(type: "int", nullable: true),
                    VIP = table.Column<int>(type: "int", nullable: true),
                    replied_msg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    my_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RSM_STAT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    New_Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsNotification", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SmsNotifyRequestResponse",
                columns: table => new
                {
                    SmsNotifyRequestResponseId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MethordName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
