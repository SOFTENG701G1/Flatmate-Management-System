using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiBackend.Migrations
{
    public partial class PropertyFixMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Schedule");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Schedule",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
