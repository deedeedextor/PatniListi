namespace PatniListi.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RemovedOnDeleteRestrictedBehaviourForCarUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "CarUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "CarUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "CarUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CarUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "CarUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "CarUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarUsers_IsDeleted",
                table: "CarUsers",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CarUsers_IsDeleted",
                table: "CarUsers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "CarUsers");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "CarUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CarUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CarUsers");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "CarUsers");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "CarUsers");
        }
    }
}
