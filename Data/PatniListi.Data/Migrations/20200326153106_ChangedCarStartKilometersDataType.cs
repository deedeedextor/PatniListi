namespace PatniListi.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ChangedCarStartKilometersDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AddedQuantity",
                table: "TransportWorkTickets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EndKilometers",
                table: "TransportWorkTickets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FuelConsumption",
                table: "TransportWorkTickets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Residue",
                table: "TransportWorkTickets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "StartKilometers",
                table: "TransportWorkTickets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TravelledDistance",
                table: "TransportWorkTickets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "StartKilometers",
                table: "Cars",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedQuantity",
                table: "TransportWorkTickets");

            migrationBuilder.DropColumn(
                name: "EndKilometers",
                table: "TransportWorkTickets");

            migrationBuilder.DropColumn(
                name: "FuelConsumption",
                table: "TransportWorkTickets");

            migrationBuilder.DropColumn(
                name: "Residue",
                table: "TransportWorkTickets");

            migrationBuilder.DropColumn(
                name: "StartKilometers",
                table: "TransportWorkTickets");

            migrationBuilder.DropColumn(
                name: "TravelledDistance",
                table: "TransportWorkTickets");

            migrationBuilder.AlterColumn<int>(
                name: "StartKilometers",
                table: "Cars",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
