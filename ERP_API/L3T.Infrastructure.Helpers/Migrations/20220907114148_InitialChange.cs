using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L3T.Infrastructure.Helpers.Migrations
{
    public partial class InitialChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Tests");
        }
    }
}
