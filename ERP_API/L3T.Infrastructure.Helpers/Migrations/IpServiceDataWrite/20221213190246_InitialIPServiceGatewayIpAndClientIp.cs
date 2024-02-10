using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L3T.Infrastructure.Helpers.Migrations.IpServiceDataWrite
{
    public partial class InitialIPServiceGatewayIpAndClientIp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GatewayIpAddresses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BtsId = table.Column<long>(type: "bigint", nullable: false),
                    DistributorId = table.Column<long>(type: "bigint", nullable: false),
                    RouterType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RouterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RouterPort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GateWay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RouterHostName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RouterModelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RouterSwitchIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HostName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GatewayIpAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GatewayWiseClientIpAddresses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GatewayIpAddressId = table.Column<long>(type: "bigint", nullable: false),
                    PackageId = table.Column<long>(type: "bigint", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PoolName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubNetMask = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LookBackAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriberId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriberSlNo = table.Column<int>(type: "int", nullable: false),
                    UsedStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GatewayWiseClientIpAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GatewayWiseClientIpAddresses_GatewayIpAddresses_GatewayIpAddressId",
                        column: x => x.GatewayIpAddressId,
                        principalTable: "GatewayIpAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GatewayWiseClientIpAddresses_GatewayIpAddressId",
                table: "GatewayWiseClientIpAddresses",
                column: "GatewayIpAddressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GatewayWiseClientIpAddresses");

            migrationBuilder.DropTable(
                name: "GatewayIpAddresses");
        }
    }
}
