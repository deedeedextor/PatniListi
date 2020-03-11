namespace PatniListi.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ChangeApplicationUserColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoginDate",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoggingDate",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoggingDate",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }
    }
}
