namespace PatniListi.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedMoreDataAnnotationAttributesToTransportWorkTicketAndRouteTransportWorkTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedQuantity",
                table: "TransportWorkTickets");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "RouteTransportWorkTickets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "RouteTransportWorkTickets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "RouteTransportWorkTickets",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RouteTransportWorkTickets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "RouteTransportWorkTickets",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "RouteTransportWorkTickets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RouteTransportWorkTickets_IsDeleted",
                table: "RouteTransportWorkTickets",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RouteTransportWorkTickets_IsDeleted",
                table: "RouteTransportWorkTickets");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "RouteTransportWorkTickets");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "RouteTransportWorkTickets");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RouteTransportWorkTickets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RouteTransportWorkTickets");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "RouteTransportWorkTickets");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "RouteTransportWorkTickets");

            migrationBuilder.AddColumn<double>(
                name: "AddedQuantity",
                table: "TransportWorkTickets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
