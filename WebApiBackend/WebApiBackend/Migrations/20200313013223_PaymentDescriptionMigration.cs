using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiBackend.Migrations
{
    public partial class PaymentDescriptionMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Payment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Payment");
        }
    }
}
