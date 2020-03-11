namespace PatniListi.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddFuelTypeToCar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "FuelType",
                table: "Invoices");

            migrationBuilder.AddColumn<int>(
                name: "FuelType",
                table: "Cars",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FuelType",
                table: "Cars");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<int>(
                name: "FuelType",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
