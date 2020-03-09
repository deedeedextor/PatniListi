using Microsoft.EntityFrameworkCore.Migrations;

namespace PatniListi.Data.Migrations
{
    public partial class AddColumnFuelTypeForInvoices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FuelType",
                table: "Invoices",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FuelType",
                table: "Invoices");
        }
    }
}
