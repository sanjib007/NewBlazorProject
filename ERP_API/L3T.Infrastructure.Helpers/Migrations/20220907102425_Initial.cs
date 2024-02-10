using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L3T.Infrastructure.Helpers.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicketEntry",
                columns: table => new
                {
                    TicketId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    brCliCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    brSlNo = table.Column<int>(type: "int", nullable: false),
                    MqID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiveDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FaultOccured = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ComplainCategory = table.Column<int>(type: "int", nullable: false),
                    Complains = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComplainSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LedStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PendingTeamID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComplainReceiveby = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompleteBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompleteDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefeNoActivity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ForwardDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignEngineer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupportType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExecuteDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CloseCategory = table.Column<int>(type: "int", nullable: false),
                    DaysExtension = table.Column<int>(type: "int", nullable: false),
                    CauseOfDelay = table.Column<int>(type: "int", nullable: false),
                    CauseOfTermination = table.Column<int>(type: "int", nullable: false),
                    TerminateExcuteDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeviceCollectionStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GenerateTicketSupportType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PendingReasonID = table.Column<int>(type: "int", nullable: false),
                    ScheduleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    vicidial_call_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TraceLess_Schedule = table.Column<int>(type: "int", nullable: false),
                    ScheduleStatus = table.Column<int>(type: "int", nullable: false),
                    ONUSTATUS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ONULASER = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ONUBW = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ONUPORT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    USERMAC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VLAN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PON = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    REBOOT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceMobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBussinessHour = table.Column<bool>(type: "bit", nullable: false),
                    IsUrgentSupport = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceOfInformation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskCategory = table.Column<int>(type: "int", nullable: false),
                    TaskNature = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketEntry", x => x.TicketId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketEntry");
        }
    }
}
