using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L3TIdentityOAuth2Server.Migrations
{
    public partial class UpdateAppInfosTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AppInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AppInfos");
        }
    }
}
