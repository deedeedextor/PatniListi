using Microsoft.EntityFrameworkCore.Migrations;

namespace PatniListi.Data.Migrations
{
    public partial class ChangedInvoiceQuantityDataTypeToDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Invoices",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                table: "Invoices",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
