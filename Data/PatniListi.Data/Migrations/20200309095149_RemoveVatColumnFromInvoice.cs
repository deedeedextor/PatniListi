namespace PatniListi.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RemoveVatColumnFromInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VAT",
                table: "Invoices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "VAT",
                table: "Invoices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
