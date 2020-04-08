namespace PatniListi.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedColumnCurrentLitersForInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CurrentLiters",
                table: "Invoices",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentLiters",
                table: "Invoices");
        }
    }
}
