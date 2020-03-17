using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiBackend.Migrations
{
    public partial class ResetTokenField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "HashedPassword",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HaveReset",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ResetToken",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashedPassword",
                table: "User");

            migrationBuilder.DropColumn(
                name: "HaveReset",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ResetToken",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "User",
                type: "TEXT",
                nullable: true);
        }
    }
}
