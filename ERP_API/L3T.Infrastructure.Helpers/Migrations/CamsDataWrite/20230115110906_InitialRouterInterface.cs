using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L3T.Infrastructure.Helpers.Migrations.CamsDataWrite
{
    public partial class InitialRouterInterface : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MikrotikRouterInterface",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MTU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActualMTU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    L2MTU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxL2MTU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MacAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastLinkDownTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastLinkUpTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkDowns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RXByte = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TXByte = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RXPacket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TXPacket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RXDrop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TXDrop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TXQueueDrop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RXError = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TXError = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FpRxByte = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FpTxByte = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FpRxPacket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FpTxPacket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Running = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disabled = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RouterIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MikrotikRouterInterface", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MikrotikRouterInterface");
        }
    }
}
