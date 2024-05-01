using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Organizer.Migrations
{
    public partial class Tenantvalue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tenantid",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tenantid",
                table: "Tasks");
        }
    }
}
